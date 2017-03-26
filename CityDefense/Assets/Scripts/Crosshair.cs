using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private float m_edgeMargin = 0.05f;
    [SerializeField]
    private float m_minTargetHeight = 1.0f;

    private RectTransform m_rt;
    private RectTransform m_canvas;
    private Vector3 m_lastMoustPos;

    public void Init(Color color)
    {
        m_canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        m_rt = GetComponent<RectTransform>();
        
        GetComponentInChildren<Image>().color = color;
	}

    public void Move()
    {
        m_lastMoustPos = Input.mousePosition;
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(m_lastMoustPos);
        viewPos.y = viewPos.y * (1 - Camera.main.rect.y) + Camera.main.rect.y;
        Vector2 targetPos = viewPos - (Vector3.one * 0.5f);

        float sideMargin = 0.5f - (m_edgeMargin * (m_canvas.sizeDelta.y / m_canvas.sizeDelta.x));
        float topMargin = 0.5f - m_edgeMargin;
        float bottomMargin = (Camera.main.WorldToViewportPoint(Vector3.up * m_minTargetHeight).y * (1 - Camera.main.rect.y) + Camera.main.rect.y) - 0.5f;
        float x = Mathf.Clamp(targetPos.x, -sideMargin, sideMargin);
        float y = Mathf.Clamp(targetPos.y, bottomMargin, topMargin);
        m_rt.anchoredPosition = new Vector2(x * m_canvas.sizeDelta.x, y * m_canvas.sizeDelta.y);
    }

    public Vector3 GetTargetPos()
    {
        Vector3 crosshairPos = new Vector3(
            (m_rt.anchoredPosition.x / m_canvas.sizeDelta.x) + 0.5f,
            (m_rt.anchoredPosition.y / m_canvas.sizeDelta.y) + 0.5f,
            0);

        crosshairPos.y = (crosshairPos.y - Camera.main.rect.y) / (1 - Camera.main.rect.y);
        Ray ray = Camera.main.ViewportPointToRay(crosshairPos);
        float distance = Vector3.Dot(-ray.origin, Vector3.back) / Vector3.Dot(ray.direction, Vector3.back);
        return distance * ray.direction + ray.origin;
    }
}

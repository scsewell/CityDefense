using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private float m_edgeMargin = 20.0f;
    [SerializeField]
    private float m_edgeMarginBottom = 100.0f;

    private RectTransform m_rt;
    private RectTransform m_canvas;
    private bool m_useMouse;
    private Vector3 m_lastMoustPos;

    public void Init(Color color, bool useMouse)
    {
        m_canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        m_rt = GetComponent<RectTransform>();

        m_useMouse = useMouse;

        GetComponentInChildren<Image>().color = color;
	}

    public void Move()
    {
        Vector2 targetPos;

        if (m_useMouse && Input.mousePosition != m_lastMoustPos)
        {
            m_lastMoustPos = Input.mousePosition;
            Vector2 mousePos = Camera.main.ScreenToViewportPoint(m_lastMoustPos) - (Vector3.one * 0.5f);
            targetPos = new Vector2(mousePos.x * m_canvas.sizeDelta.x, mousePos.y * m_canvas.sizeDelta.y);
        }
        else
        {
            Vector2 input = Vector2.ClampMagnitude(new Vector2(Controls.Instance.AverageValue(GameAxis.TrackX), Controls.Instance.AverageValue(GameAxis.TrackY)), 1);
            Vector2 delta = (input * input.magnitude * input.magnitude) * (speed * m_canvas.sizeDelta.y) * Time.deltaTime;
            targetPos = m_rt.anchoredPosition + delta;
        }

        float sideMargin = (m_canvas.sizeDelta.x / 2) - m_edgeMargin;
        float topMargin = (m_canvas.sizeDelta.y / 2) - m_edgeMargin;
        float bottomMargin = (m_canvas.sizeDelta.y / 2) - m_edgeMarginBottom;
        m_rt.anchoredPosition = new Vector2(Mathf.Clamp(targetPos.x, -sideMargin, sideMargin), Mathf.Clamp(targetPos.y, -bottomMargin, topMargin));
    }

    public Vector3 GetTargetPos()
    {
        Vector3 crosshairPos = new Vector3(
            (m_rt.anchoredPosition.x / m_canvas.sizeDelta.x) + 0.5f,
            (m_rt.anchoredPosition.y / m_canvas.sizeDelta.y) + 0.5f,
            0);

        Ray ray = Camera.main.ViewportPointToRay(crosshairPos);
        float distance = Vector3.Dot(-ray.origin, Vector3.back) / Vector3.Dot(ray.direction, Vector3.back);
        return distance * ray.direction + ray.origin;
    }
}

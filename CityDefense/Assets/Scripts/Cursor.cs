using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private float m_edgeMargin = 20.0f;
    [SerializeField]
    private float m_edgeMarginBottom = 100.0f;

    private RectTransform m_rt;
    private RectTransform m_canvas;

    public void Init(Color color)
    {
        m_canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        m_rt = GetComponent<RectTransform>();

        GetComponentInChildren<Image>().color = color;
	}

    public void Move()
    {
        Vector2 input = new Vector2(Controls.Instance.AverageValue(GameAxis.TrackX), Controls.Instance.AverageValue(GameAxis.TrackY));
        Vector2 delta = Vector2.ClampMagnitude(input * input.magnitude * input.magnitude, 1) * (speed * m_canvas.sizeDelta.y) * Time.deltaTime;

        float sideMargin    = (m_canvas.sizeDelta.x / 2) - m_edgeMargin;
        float topMargin     = (m_canvas.sizeDelta.y / 2) - m_edgeMargin;
        float bottomMargin  = (m_canvas.sizeDelta.y / 2) - m_edgeMarginBottom;
        float x = Mathf.Clamp(m_rt.anchoredPosition.x + delta.x, -sideMargin, sideMargin);
        float y = Mathf.Clamp(m_rt.anchoredPosition.y + delta.y, -bottomMargin, topMargin);
        m_rt.anchoredPosition = new Vector2(x, y);
    }

    public Vector3 GetTargetPos()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(
            (m_rt.anchoredPosition.x / m_canvas.sizeDelta.x) + 0.5f,
            (m_rt.anchoredPosition.y / m_canvas.sizeDelta.y) + 0.5f,
            -Camera.main.transform.position.z)
            );
    }
}

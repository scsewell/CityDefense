using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField]
    private Image m_mainBar;
    [SerializeField]
    private Image m_damageBar;
    [SerializeField]
    private Color[] m_colors;
    [SerializeField]
    private Color m_damageBarColor;
    [SerializeField]
    private Color m_damageFlashColor;

    [SerializeField]
    [Range(0, 16)]
    private float m_flashFadeSpeed = 4f;
    [SerializeField]
    [Range(0, 2)]
    private float m_damageWaitTime = 1f;
    [SerializeField]
    [Range(0, 2)]
    private float m_damageFadeTime = 1f;
    
    private Health m_health;
    private RectTransform m_canvas;
    private RectTransform m_rt;
    private float m_hurtFac = 0;
    private float m_damageTime = 0;
    
    public void SetSource(Health health)
    {
        m_canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        m_rt = GetComponent<RectTransform>();
        m_health = health;
        m_health.OnDamage += Hurt;
        Init();
    }

    public void Init()
    {
        m_hurtFac = 0;
        m_damageTime = 0;
    }

    private void OnDestroy()
    {
        if (m_health)
        {
            m_health.OnDamage -= Hurt;
        }
    }

    private void Hurt(float healthFractionLost)
    {
        m_hurtFac = 1;
        m_damageBar.fillAmount = m_mainBar.fillAmount;
        m_damageTime = Time.time;
    }

    private void LateUpdate()
    {
        float healthFraction = Mathf.Clamp01(m_health.HealthFraction);
        m_mainBar.fillAmount = healthFraction;

        float arrayPos = (1 - healthFraction) * (m_colors.Length - 1);
        int firstColorIndex = (int)Mathf.Floor(arrayPos);
        int secondColorIndex = (int)Mathf.Ceil(arrayPos);
        float fac = arrayPos % 1;
        Color healthColor = Color.Lerp(m_colors[firstColorIndex], m_colors[secondColorIndex], fac);

        m_mainBar.color = Color.Lerp(healthColor, m_damageFlashColor, m_hurtFac);

        m_hurtFac = Mathf.MoveTowards(m_hurtFac, 0, m_flashFadeSpeed * Time.deltaTime);

        float alpha = 1 - ((Time.time - (m_damageTime + m_damageWaitTime)) / m_damageFadeTime);
        SetAlpha(m_damageBar, m_damageBarColor, alpha);
    }

    private void SetAlpha(Image image, Color color, float alpha)
    {
        Color col = color;
        col.a *= Mathf.Clamp01(alpha);
        image.color = col;
    }

    public void UpdatePosition(Vector3 worldPos)
    {
        Vector3 viewSpace = Camera.main.WorldToViewportPoint(worldPos);
        viewSpace.y = viewSpace.y * (1 - Camera.main.rect.y) + Camera.main.rect.y;
        m_rt.anchoredPosition = new Vector2((viewSpace.x - 0.5f) * m_canvas.sizeDelta.x, (viewSpace.y - 0.5f) * m_canvas.sizeDelta.y);
    }
}

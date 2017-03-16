using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_damageBarPrefab;
    [SerializeField]
    private RectTransform m_damageBarParent;
    [SerializeField]
    private Image m_mainBarImage;
    [SerializeField]
    private Color[] m_colors;
    [SerializeField]
    private Color m_hurtColor;

    [SerializeField]
    [Range(0, 16)]
    private float m_hurtFadeSpeed = 4f;
    [SerializeField]
    [Range(0, 2)]
    private float m_damageWaitTime = 1f;
    [SerializeField]
    [Range(0, 2)]
    private float m_damageFadeTime = 1f;
    
    private Health m_health;
    private RectTransform m_canvas;
    private RectTransform m_rt;
    private Dictionary<Image, float> m_damageBars;
    private Vector3 m_offset;
    private float m_hurtFac = 0;


    public void Init(Health health, Vector3 offset)
    {
        m_canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        m_rt = GetComponent<RectTransform>();

        m_health = health;
        m_health.OnHurt += Hurt;

        m_offset = offset;

        m_damageBars = new Dictionary<Image, float>();
        foreach (Transform child in m_damageBarParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (m_health)
        {
            m_health.OnHurt -= Hurt;
        }
    }

    private void Hurt(float healthFractionLost)
    {
        m_hurtFac = 1;

        RectTransform damageBar = Instantiate(m_damageBarPrefab, m_damageBarParent) as RectTransform;
        damageBar.localScale = Vector3.one;
        damageBar.localPosition = Vector2.zero;
        damageBar.anchoredPosition = Vector2.zero;
        damageBar.sizeDelta = Vector2.zero;
        damageBar.anchorMin = Vector2.zero;
        damageBar.anchorMax = Vector2.one;

        Image damageImage = damageBar.GetComponent<Image>();
        damageImage.fillAmount = m_mainBarImage.fillAmount;
        m_damageBars.Add(damageImage, Time.time);
    }

    private void LateUpdate()
    {
        if (m_health)
        {
            SetHeath(m_health.HealthFraction);
            Vector3 viewSpace = Camera.main.WorldToViewportPoint(m_health.transform.position + m_offset);
            m_rt.anchoredPosition = new Vector2((viewSpace.x - 0.5f) * m_canvas.sizeDelta.x, (viewSpace.y - 0.5f) * m_canvas.sizeDelta.y);
        }

        m_hurtFac = Mathf.Lerp(m_hurtFac, 0, m_hurtFadeSpeed * Time.deltaTime);

        List<Image> damageBars = new List<Image>(m_damageBars.Keys);
        foreach (Image damageBar in damageBars)
        {
            float alpha = 0.25f - (0.25f * (Time.time - (m_damageBars[damageBar] + m_damageWaitTime)) / m_damageFadeTime);
            if (alpha > 0)
            {
                SetAlpha(damageBar, alpha);
            }
            else
            {
                m_damageBars.Remove(damageBar);
                Destroy(damageBar.gameObject);
            }
        }
    }

    public void SetHeath(float healthFraction)
    {
        healthFraction = Mathf.Clamp01(healthFraction);
        m_mainBarImage.fillAmount = healthFraction;

        float arrayPos = (1 - healthFraction) * (m_colors.Length - 1);
        int firstColorIndex = (int)Mathf.Floor(arrayPos);
        int secondColorIndex = (int)Mathf.Ceil(arrayPos);
        float fac = arrayPos % 1;
        Color healthColor = Color.Lerp(m_colors[firstColorIndex], m_colors[secondColorIndex], fac);

        m_mainBarImage.color = Color.Lerp(healthColor, m_hurtColor, m_hurtFac);
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color col = image.color;
        col.a = Mathf.Clamp01(alpha);
        image.color = col;
    }
}

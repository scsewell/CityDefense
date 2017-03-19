using UnityEngine;

public class BGManager : Singleton<BGManager>
{
    [SerializeField]
    private Light m_sun;
    [SerializeField]
    private Light m_moon;
    [SerializeField]
    private ParticleSystem m_stars;
    [SerializeField]
    private ReflectionProbe m_probe;
    
    [SerializeField]
    private float m_nightDay = 0;

    [SerializeField]
    private Gradient m_sunColor;
    [SerializeField]
    private AnimationCurve m_sunIntensity;
    [SerializeField]
    private AnimationCurve m_moonIntensity;

    [SerializeField]
    private Gradient m_ambientColor;
    [SerializeField]
    private AnimationCurve m_ambientIntensity;

    [SerializeField]
    private AnimationCurve m_starIntensity;

    private Transform m_sunPivot;
    private Material m_starsMat;
    private float m_lastNightDay;

    private void Start()
    {
        m_starsMat = m_stars.GetComponent<ParticleSystemRenderer>().material;
        m_sunPivot = m_sun.transform.parent.transform;
    }

    private void Update()
    {
        if (m_nightDay != m_lastNightDay)
        {
            m_sunPivot.rotation = Quaternion.AngleAxis((1 - m_nightDay) * 180, Vector3.right);
            float dot = 0.5f * Vector3.Dot(m_sun.transform.forward, Vector3.down) + 0.5f;

            m_sun.intensity = m_sunIntensity.Evaluate(dot);
            m_moon.intensity = m_moonIntensity.Evaluate(dot);
            m_sun.color = m_sunColor.Evaluate(dot);
            RenderSettings.ambientIntensity = m_ambientIntensity.Evaluate(dot);
            RenderSettings.ambientLight = m_ambientColor.Evaluate(dot);
            m_starsMat.SetColor("_TintColor", new Color(1, 1, 1, m_starIntensity.Evaluate(dot)));

            m_probe.RenderProbe();

            m_lastNightDay = m_nightDay;
        }
    }
}

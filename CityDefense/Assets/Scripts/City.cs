using UnityEngine;

[RequireComponent(typeof(Health))]
public class City : MonoBehaviour
{
    [SerializeField]
    private float m_healAmount = 25;
    [SerializeField]
    private float m_healPeriod = 5;
    [SerializeField]
    private Healthbar m_healthbarPrefab;
    [SerializeField]
    private Vector3 m_healthbarOffset;

    private GameUI m_ui;
    private Health m_health;
    private Healthbar m_healthbar;
    private float m_lastHealTime;

	private void Start()
    {
        m_health = GetComponent<Health>();

        m_healthbar = GameUI.Instance.AddHealthbar(m_healthbarPrefab);
        m_healthbar.Init(m_health);
    }
    
    private void FixedUpdate()
    {
        if (Time.time - m_healPeriod > m_lastHealTime)
        {
            m_health.Heal(m_healAmount);
            m_lastHealTime = Time.time;
        }
    }

    private void LateUpdate()
    {
        m_healthbar.UpdatePosition(transform.position + m_healthbarOffset);
    }
}

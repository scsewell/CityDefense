using UnityEngine;

[RequireComponent(typeof(Health))]
public class Target : MonoBehaviour
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

        OnStart();
    }
    
    private void FixedUpdate()
    {
        if (Time.time - m_healPeriod > m_lastHealTime)
        {
            m_health.Heal(m_healAmount);
            m_lastHealTime = Time.time;
        }
        OnFixedUpdate();
    }

    private void LateUpdate()
    {
        m_healthbar.UpdatePosition(transform.position + m_healthbarOffset);
        OnLateUpdate();
    }

    public virtual Vector3 GetTargetPos()
    {
        return transform.position;
    }

    protected virtual void OnStart() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnLateUpdate() { }
}

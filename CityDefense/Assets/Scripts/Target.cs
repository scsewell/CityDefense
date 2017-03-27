using UnityEngine;

[RequireComponent(typeof(Health))]
public class Target : PooledObject
{
    [SerializeField]
    private float m_healAmount = 25;
    [SerializeField]
    private float m_healPeriod = 5;
    [SerializeField]
    private Healthbar m_healthbarPrefab;
    [SerializeField]
    private Vector3 m_healthbarOffset;
    
    protected Health m_health;
    protected Healthbar m_healthbar;
    private float m_lastHealTime;

    private void Awake()
    {
        m_health = GetComponent<Health>();
        m_healthbar = GameUI.Instance.AddHealthbar(m_healthbarPrefab);
        m_healthbar.SetSource(m_health);
        OnAwake();
    }

    private void Start()
    {
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

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnLateUpdate() { }
}

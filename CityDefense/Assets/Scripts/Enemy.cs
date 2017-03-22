using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : PooledObject
{
    [SerializeField]
    private int m_score;
    public int Score
    {
        get { return m_score; }
    }

    [SerializeField]
    private float m_damage;
    public float Damage
    {
        get { return m_damage; }
    }

    private Health m_health;

    protected virtual void Awake()
    {
        m_health = GetComponent<Health>();
        m_health.OnDie += OnDie;

        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
    }

    protected virtual void OnDestroy()
    {
        m_health.OnDie -= OnDie;
    }

    protected virtual void OnEnable()
    {
        m_health.ResetHealth();
        EnemyManager.Instance.AddEnemy(this);
    }

    protected virtual void OnDisable()
    {
        EnemyManager.Instance.RemoveEnemy(this);
    }

    private void OnDie()
    {
        Deactivate();
        OnDestroyed();
    }
    protected virtual void OnDestroyed() { }

    public void EnemyUpdate()
    {
        OnUpdate();
    }
    protected virtual void OnUpdate() { }

    public void EnemyFixedUpdate()
    {
        OnFixedUpdate();
    }
    protected virtual void OnFixedUpdate() { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            other.gameObject.GetComponentInParent<Health>().ApplyDamage(m_damage);
            OnDie();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (m_health.ApplyDamage(projectile.GetDamage()))
            {
                projectile.Owner.EnemyDestoryed(this);
            }
            projectile.Deactivate();
        }
    }
}
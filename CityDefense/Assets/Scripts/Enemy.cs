using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : PooledObject
{
    [SerializeField]
    private int m_score;
    [SerializeField]
    private float m_damage;

    private Health m_health;

    protected virtual void Awake()
    {
        m_health = GetComponent<Health>();
        m_health.OnDie += OnDie;

        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
    }

    private void OnDestroy()
    {
        m_health.OnDie -= OnDie;
    }

    private void OnEnable()
    {
        m_health.ResetHealth();
    }

    private void OnDie()
    {
        Deactivate();
        OnDestroyed();
    }

    protected virtual void OnDestroyed() {}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            other.gameObject.GetComponent<Health>().ApplyDamage(m_damage);
            OnDie();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            m_health.ApplyDamage(projectile.GetDamage());
            projectile.Deactivate();
        }
    }
}
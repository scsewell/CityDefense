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

    protected virtual void OnDestroy()
    {
        m_health.OnDie -= OnDie;
    }

    protected virtual void OnEnable()
    {
        m_health.ResetHealth();
    }

    private void OnDie()
    {
        Deactivate();
        OnDestroyed();
    }

    protected virtual void OnDestroyed() {}

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
                projectile.Owner.IncreaseScore(m_score);
            }
            projectile.Deactivate();
        }
    }
}
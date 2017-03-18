using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float m_maxHealth = 100;
    public float MaxHealth
    {
        get { return m_maxHealth; }
    }

    private float m_currentHealth;
    public float CurrentHealth
    {
        get { return m_currentHealth; }
    }

    public delegate void DeathHandler();
    public event DeathHandler OnDie;

    public delegate void DamageHandler(float healthFractionLost);
    public event DamageHandler OnDamage;

    private void Awake()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        m_currentHealth = m_maxHealth;
    }

    public bool IsAlive
    {
        get { return m_currentHealth > 0; }
    }
    
    public float HealthFraction
    {
        get { return m_currentHealth / m_maxHealth; }
    }

    public void ApplyDamage(float damage)
    {
        if (!IsAlive)
        {
            return;
        }
        m_currentHealth = Mathf.Max(m_currentHealth - damage, 0);
        if (OnDamage != null)
        {
            OnDamage(damage / m_maxHealth);
        }
        if (OnDie != null && !IsAlive)
        {
            OnDie();
        }
    }

    public void Heal(float healthGained)
    {
        if (!IsAlive)
        {
            return;
        }
        m_currentHealth = Mathf.Min(m_currentHealth + healthGained, m_maxHealth);
    }
}

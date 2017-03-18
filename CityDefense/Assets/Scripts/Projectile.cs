using UnityEngine;

public class Projectile : PooledObject
{
    [SerializeField]
    private int m_cost;
    [SerializeField]
    private float m_damage;

    private Player m_owner;
    public Player Owner
    {
        get { return m_owner; }
    }

    public virtual void Init(Player owner)
    {
        m_owner = owner;
    }

    public int GetCost()
    {
        return m_cost;
    }

    public float GetDamage()
    {
        return m_damage;
    }
}

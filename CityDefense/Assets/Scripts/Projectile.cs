using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int m_cost;
    [SerializeField]
    private float m_damage;

    private ObjectPool m_pool;

    public void Init(Player owner, ObjectPool pool)
    {
        m_pool = pool;
        OnInit(owner);
    }

    protected virtual void OnInit(Player owner)
    {
    }

    public int GetCost()
    {
        return m_cost;
    }

    public float GetDamage()
    {
        return m_damage;
    }

    public void Destroy()
    {
        m_pool.DestroyObject(gameObject);
    }
}

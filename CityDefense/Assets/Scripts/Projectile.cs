using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int m_cost;
    [SerializeField]
    private float m_damage;

    public int GetCost()
    {
        return m_cost;
    }

    public float GetDamage()
    {
        return m_damage;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class ParticleKill : MonoBehaviour
{
    private static List<ObjectPool> m_pool = new List<ObjectPool>();

    private ParticleSystem m_particles;

    private void Awake()
    {
        m_particles = GetComponentInChildren<ParticleSystem>();
    }
    
    public void Init(ObjectPool pool)
    {
        //m_pool = pool;
    }

    private void Update()
    {
        if (!m_particles.IsAlive())
        {
            //m_pool.DestroyObject(gameObject);
        }
    }
}
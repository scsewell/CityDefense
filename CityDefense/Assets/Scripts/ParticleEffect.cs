using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ParticleEffect : PooledObject
{
    private ParticleSystem m_particleSystem;

    private void Awake()
    {
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
    }
    
    public void Init()
    {
        m_particleSystem.Clear(true);
        m_particleSystem.Play(true);
    }

    private void Update()
    {
        if (!m_particleSystem.IsAlive(false))
        {
            Deactivate();
        }
    }
}
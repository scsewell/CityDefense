using UnityEngine;

public class ParticleKill : MonoBehaviour
{
    private ParticleSystem m_particles;

    private void Awake()
    {
        m_particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (!m_particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}

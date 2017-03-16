using UnityEngine;

public class ParticleKill : MonoBehaviour
{
    private void Update()
    {
        if (!GetComponentInChildren<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }
}

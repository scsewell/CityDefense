using UnityEngine;

public class Factory : Target
{
    [SerializeField]
    private Transform m_truckSpawn;
    [SerializeField]
    private float m_truckSpawnRate = 2;
    [SerializeField]
    private float m_truckDoorWait = 6;
    private float m_lastTruckSpawn;

    [SerializeField]
    private float m_targetWidth = 1.25f;

    private Animator m_anim;

    protected override void OnAwake()
    {
        m_anim = GetComponentInChildren<Animator>();
    }

    protected override void OnFixedUpdate()
    {
        if (!GameManager.Instance.RoundOver)
        {
            float timeSinceTruck = GameManager.Instance.RoundTime - m_lastTruckSpawn;
            if (timeSinceTruck > (60f / m_truckSpawnRate))
            {
                PoolManager.GetTruck(m_truckSpawn.position, m_truckSpawn.rotation);
                m_anim.SetBool("DoorOpen", true);
                m_lastTruckSpawn = GameManager.Instance.RoundTime;
            }
            else if (timeSinceTruck > m_truckDoorWait)
            {
                m_anim.SetBool("DoorOpen", false);
            }
        }
    }

    public override Vector3 GetTargetPos()
    {
        return transform.position + Vector3.right * Random.Range(-m_targetWidth, m_targetWidth);
    }
}

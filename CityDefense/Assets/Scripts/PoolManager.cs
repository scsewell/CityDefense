using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    private PooledObject[] m_bullet1Prefabs;
    [SerializeField]
    private int m_bullet1PoolSize = 10;
    private static ObjectPool m_bullet1Pool;

    [SerializeField]
    private PooledObject[] m_truckPrefabs;
    [SerializeField]
    private int m_truckPoolSize = 10;
    private static ObjectPool m_truckPool;

    [SerializeField]
    private PooledObject[] m_explosion1Prefabs;
    [SerializeField]
    private int m_explosion1PoolSize = 10;
    private static ObjectPool m_explosion1Pool;

    [SerializeField]
    private PooledObject[] m_rocketPrefabs;
    [SerializeField]
    private int m_rocketPoolSize = 10;
    private static ObjectPool m_rocketPool;

    private void Awake()
    {
        m_bullet1Pool = new ObjectPool(m_bullet1Prefabs, m_bullet1PoolSize);
        m_truckPool = new ObjectPool(m_truckPrefabs, m_truckPoolSize);
        m_explosion1Pool = new ObjectPool(m_explosion1Prefabs, m_explosion1PoolSize);
        m_rocketPool = new ObjectPool(m_rocketPrefabs, m_rocketPoolSize);
    }

    public static Bullet GetBullet1(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        return m_bullet1Pool.GetObject(position, rotation).GetComponent<Bullet>();
    }

    public static Truck GetTruck(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        return m_truckPool.GetObject(position, rotation).GetComponent<Truck>();
    }

    public static ParticleEffect GetExplosion1(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        return m_explosion1Pool.GetObject(position, rotation).GetComponent<ParticleEffect>();
    }

    public static Rocket GetRocket(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        return m_rocketPool.GetObject(position, rotation).GetComponent<Rocket>();
    }
}

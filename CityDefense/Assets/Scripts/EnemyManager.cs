using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private Target m_city;

    [SerializeField] private Target m_factory;
    [Range(0, 1)]
    [SerializeField] private float m_factoryTargetChance = 0.1f;
    
    [SerializeField] private float m_rocketStartWait = 5.0f;
    [SerializeField] private float m_rocketRate = 8.0f;
    [SerializeField] private float m_rocketCooldown = 4.0f;
    [Range(0, 0.01f)]
    [SerializeField] private float m_rocketExponent = 0.001f;
    [Range(0, 10)]
    [SerializeField] private float m_rocketSpawnWidth = 6.0f;
    [Range(10, 20)]
    [SerializeField] private float m_rocketSpawnHeight = 12.0f;
    private float m_rocketLastTime;

    private List<Enemy> m_enemies;
    private float m_roundTime = 0;

    private void Awake()
    {
        m_enemies = new List<Enemy>();
    }

    private void Update()
    {
        foreach (Enemy enemy in m_enemies)
        {
            enemy.EnemyUpdate();
        }
        GameUI.Instance.UpdateTime(m_roundTime);
    }

    private void FixedUpdate()
    {
        m_roundTime += Time.deltaTime;
    
        if (DoSpawn(m_rocketStartWait, m_rocketLastTime, m_rocketCooldown, m_rocketRate, m_rocketExponent))
        {
            Vector3 pos = new Vector3(Random.Range(-m_rocketSpawnWidth, m_rocketSpawnWidth), m_rocketSpawnHeight, 0);
            Rocket rocket = PoolManager.GetRocket(pos, Quaternion.identity);
            rocket.SetTarget(PickTarget().GetTargetPos());
            m_rocketLastTime = m_roundTime;
        }

        foreach (Enemy enemy in m_enemies)
        {
            enemy.EnemyFixedUpdate();
        }
    }

    private bool DoSpawn(float startWait, float lastTime, float cooldown, float rate, float intensity)
    {
        float time = m_roundTime - startWait;
        float exp = Mathf.Exp(Mathf.Max(intensity, 0) * time);
        return time > 0 &&
                m_roundTime - lastTime > cooldown / exp &&
                Random.value < (rate / 60.0f) * exp * Time.deltaTime;
    }

    private Target PickTarget()
    {
        if (Random.value < m_factoryTargetChance)
        {
            return m_factory;
        }
        return m_city;
    }

    public void AddEnemy(Enemy enemy)
    {
        if (!m_enemies.Contains(enemy))
        {
            m_enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        m_enemies.Remove(enemy);
    }
}

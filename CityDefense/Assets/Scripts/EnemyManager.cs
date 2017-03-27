using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
    [Range(0, 1)]
    [SerializeField] private float m_otherTargetChance = 0.1f;
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

    private GameManager m_gm;
    private List<Enemy> m_enemies;
    private List<Target> m_otherTargets;

    private void Awake()
    {
        m_gm = GameManager.Instance;
        m_enemies = new List<Enemy>();
        m_otherTargets = new List<Target>();
    }

    private void Update()
    {
        foreach (Enemy enemy in m_enemies)
        {
            enemy.EnemyUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (DoSpawn(m_rocketStartWait, m_rocketLastTime, m_rocketCooldown, m_rocketRate, m_rocketExponent))
        {
            Vector3 pos = new Vector3(Random.Range(-m_rocketSpawnWidth, m_rocketSpawnWidth), m_rocketSpawnHeight, 0);
            Rocket rocket = PoolManager.GetRocket(pos, Quaternion.identity);

            Target target = PickTarget();
            Vector3 targetPos = target.GetTargetPos();
            if (targetPos != Vector3.zero)
            {
                rocket.SetTarget(targetPos);
            }
            else
            {
                rocket.SetTarget(PickTarget());
            }

            m_rocketLastTime = m_gm.RoundTime;
        }

        foreach (Enemy enemy in m_enemies)
        {
            enemy.EnemyFixedUpdate();
        }
    }

    private bool DoSpawn(float startWait, float lastTime, float cooldown, float rate, float intensity)
    {
        float time = m_gm.RoundTime - startWait;
        float exp = Mathf.Exp(Mathf.Max(intensity, 0) * time);
        return time > 0 &&
                m_gm.RoundTime - lastTime > cooldown / exp &&
                Random.value < (rate / 60.0f) * exp * Time.deltaTime;
    }

    private Target PickTarget()
    {
        if (m_otherTargets.Count > 0 && Random.value < m_otherTargetChance)
        {
            return m_otherTargets[Random.Range(0, m_otherTargets.Count)];
        }
        else if (m_gm.Factory.Health.IsAlive && Random.value < m_factoryTargetChance)
        {
            return m_gm.Factory;
        } 
        return m_gm.City;
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

    public void AddTarget(Target target)
    {
        if (!m_otherTargets.Contains(target))
        {
            m_otherTargets.Add(target);
        }
    }

    public void RemoveTarget(Target target)
    {
        m_otherTargets.Remove(target);
    }
}

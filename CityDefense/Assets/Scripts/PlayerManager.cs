using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private Color m_color;
    [SerializeField]
    private Turret m_turretPrefab;
    
    private int m_money = 0;
    public int Money
    {
        get { return m_money; }
    }

    private int m_score = 0;
    public int Score
    {
        get { return m_score; }
    }
    
    private Crosshair m_cursor;
    private List<Turret> m_turrets;

    private void Start()
    {
        m_turrets = new List<Turret>();
        m_turrets.Add(Instantiate(m_turretPrefab, new Vector3(-4, 0, 0), Quaternion.identity));
        m_turrets.Add(Instantiate(m_turretPrefab, new Vector3(3, 0, 0), Quaternion.identity));

        m_cursor = GameUI.Instance.AddCrosshair();
        m_cursor.Init(m_color);
    }

    private void FixedUpdate()
    {
        m_cursor.Move();

        bool fire = Controls.Instance.IsDown(GameButton.Fire1);
        foreach (Turret t in m_turrets)
        {
            Vector3 targetPos = m_cursor.GetTargetPos();
            t.StateUpdate(targetPos);
            if (fire)
            {
                t.FireBullet(targetPos);
            }
        }
    }

    public void EnemyDestroyed(Enemy enemy)
    {
        m_score += enemy.Score;
        m_money += enemy.Score;
        GameUI.Instance.UpdateMoney(m_money);
    }
}

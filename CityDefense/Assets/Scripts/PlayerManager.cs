﻿using UnityEngine;
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

    private int m_ammoBullet1 = 100;
    public int AmmoBullet1
    {
        get { return m_ammoBullet1; }
    }

    private Crosshair m_cursor;
    private List<Turret> m_turrets;

    private void Start()
    {
        m_turrets = new List<Turret>();
        m_turrets.Add(Instantiate(m_turretPrefab, new Vector3(-4, 0, 0.2f), Quaternion.identity));
        m_turrets.Add(Instantiate(m_turretPrefab, new Vector3(3, 0, 0.2f), Quaternion.identity));

        m_cursor = GameUI.Instance.AddCrosshair();
        m_cursor.Init(m_color);

        GameUI.Instance.UpdateScore(m_score);
        GameUI.Instance.UpdateMoney(m_money);
        GameUI.Instance.UpdateAmmoBullet1(m_ammoBullet1);
    }

    private void FixedUpdate()
    {
        m_cursor.Move();

        bool fire1 = Controls.Instance.IsDown(GameButton.Fire1);

        foreach (Turret t in m_turrets)
        {
            Vector3 targetPos = m_cursor.GetTargetPos();
            t.StateUpdate(targetPos);
            if (fire1 && m_ammoBullet1 > 0)
            {
                t.FireBullet(targetPos);
            }
        }
    }

    public void Bullet1AmmoArrived()
    {
        m_ammoBullet1 += 100;
        GameUI.Instance.UpdateAmmoBullet1(m_ammoBullet1);
    }

    public void Bullet1Fired()
    {
        m_ammoBullet1--;
        GameUI.Instance.UpdateAmmoBullet1(m_ammoBullet1);
    }

    public void EnemyDestroyed(Enemy enemy)
    {
        m_score += enemy.Score;
        m_money += enemy.Score;
        GameUI.Instance.UpdateScore(m_score);
        GameUI.Instance.UpdateMoney(m_money);
    }
}

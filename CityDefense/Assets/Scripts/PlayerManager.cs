using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private Color[] m_playerColors;
    [SerializeField]
    private Turret m_turretPrefab;
    
    private List<Player> m_players;
    private int m_money;

	private void Start()
    {
        m_players = new List<Player>();
        
        for (int i = 0; i < 1; i++)
        {
            List<Turret> turrets = new List<Turret>();
            turrets.Add(Instantiate(m_turretPrefab, new Vector3(-4, 0, 0), Quaternion.identity));
            turrets.Add(Instantiate(m_turretPrefab, new Vector3(3, 0, 0), Quaternion.identity));

            m_players.Add(new Player(i, m_playerColors[i], GameUI.Instance.AddCrosshair(), turrets));
        }
    }

    private void FixedUpdate()
    {
        foreach (Player player in m_players)
        {
            player.Update();
        }
    }

    public int GetMoney()
    {
        return m_money;
    }

    public void AddMoney(int money)
    {
        m_money += Mathf.Max(money, 0);
        GameUI.Instance.UpdateMoney(m_money);
    }
}

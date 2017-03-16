using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Color[] m_playerColors;
    [SerializeField]
    private Cursor m_cursorPrefab;
    [SerializeField]
    private Turret m_turretPrefab;

    private List<Player> m_players;

	private void Start()
    {
        m_players = new List<Player>();

        Transform ui = GameObject.FindGameObjectWithTag("UI").transform;
        Transform cursors = new GameObject("Cursors").transform;
        cursors.SetParent(ui, false);

        for (int i = 0; i < 1; i++)
        {
            Cursor cursor = Instantiate(m_cursorPrefab, cursors, false);

            List<Turret> turrets = new List<Turret>();
            turrets.Add(Instantiate(m_turretPrefab, new Vector3(-4, 0, 0), Quaternion.identity));
            turrets.Add(Instantiate(m_turretPrefab, new Vector3(3, 0, 0), Quaternion.identity));

            m_players.Add(new Player(i, m_playerColors[i], cursor, turrets));
        }
    }

    private void FixedUpdate()
    {
        foreach (Player player in m_players)
        {
            player.Update();
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Health))]
public class City : MonoBehaviour
{
    [SerializeField]
    private Healthbar m_healthbarPrefab;
    [SerializeField]
    private Vector3 m_healthbarOffset;

    private UI m_ui;
    private Health m_health;
    private Healthbar m_healthbar;

	private void Start()
    {
        m_ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();

        m_health = GetComponent<Health>();

        m_healthbar = m_ui.AddHealthbar(m_healthbarPrefab);
        m_healthbar.Init(m_health, m_healthbarOffset);
    }
    
    private void FixedUpdate()
    {
    }
}

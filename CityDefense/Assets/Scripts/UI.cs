using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Cursor m_cursorPrefab;

    private Transform m_cursors;
    private Transform m_healthbars;
    
    private void Awake()
    {
        m_cursors = new GameObject("Cursors").transform;
        m_cursors.SetParent(transform, false);

        m_healthbars = new GameObject("Healthbars").transform;
        m_healthbars.SetParent(transform, false);
    }
    
    private void Update()
    {
		
	}

    public Cursor AddCursor()
    {
        return Instantiate(m_cursorPrefab, m_cursors, false);
    }

    public Healthbar AddHealthbar(Healthbar healthbar)
    {
        return Instantiate(healthbar, m_healthbars, false);
    }
}

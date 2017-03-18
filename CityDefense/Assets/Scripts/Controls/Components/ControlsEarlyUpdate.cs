using UnityEngine;

public class ControlsEarlyUpdate : Singleton<ControlsEarlyUpdate>
{
    ControlsUpdate m_controls;

	private void Awake()
    {
        m_controls = GetComponent<ControlsUpdate>();
    }
	
	private void Update()
    {
        m_controls.EarlyUpdate();
    }
}

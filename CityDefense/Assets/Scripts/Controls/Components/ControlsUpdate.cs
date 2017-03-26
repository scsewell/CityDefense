using UnityEngine;

[RequireComponent(typeof(ControlsEarlyUpdate))]
public class ControlsUpdate : Singleton<ControlsUpdate>
{
    private void Awake()
    {
        Controls.Instance = Controls.Load();
    }
    
    private void FixedUpdate()
    {
        Controls.Instance.FixedUpdate();
    }

    public void EarlyUpdate()
    {
        Controls.Instance.EarlyUpdate();
    }

    public void Update()
    {
        Controls.Instance.Update();
    }
}

using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraSettingsManager : MonoBehaviour
{
    private PostProcessingProfile m_postProfile;

    private void Awake()
    {
        m_postProfile = GetComponent<PostProcessingBehaviour>().profile;
    }

    private void Update()
    {
        m_postProfile.antialiasing.enabled = !Settings.Instance.GetAntialiasing();
        m_postProfile.motionBlur.enabled = Settings.Instance.GetMotionBlur();
        m_postProfile.bloom.enabled = Settings.Instance.GetBloom();
    }
}
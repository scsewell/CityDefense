using UnityEngine;
using System;
using System.Collections.Generic;

public class Settings
{
    // limits on values
    public static int[] TARGET_FRAME_RATES = { 10, 30, 60, 120, 144, 500 };
    public enum ShadowQualityLevels { OFF, LOW, HIGH };

    // default values
    private const bool DEF_FULLSCREEN = true;
    private const bool DEF_VSYNC = true;
    private const bool DEF_AA = true;
    private const bool DEF_BLOOM = true;
    private const bool DEF_MOTION_BLUR = true;
    private const bool DEF_SHOW_FPS = false;
    private const int DEF_FRAMERATE = 60;
    private const float DEF_VOLUME = 1;
    private const ShadowQualityLevels DEF_SHADOW_QUALITY = ShadowQualityLevels.HIGH;


    // settings
    private bool m_fullscreen;
    public bool GetFullscreen()
    {
        return m_fullscreen;
    }
    public void SetFullscreen(bool value)
    {
        m_fullscreen = value;
    }
    
    private bool m_showFPS;
    public bool GetShowFPS()
    {
        return m_showFPS;
    }
    public void SetShowFPS(bool value)
    {
        m_showFPS = value;
    }

    private bool m_antialiasing;
    public bool GetAntialiasing()
    {
        return m_antialiasing;
    }
    public void SetAntialiasing(bool value)
    {
        m_antialiasing = value;
    }

    private bool m_vsync;
    public bool GetVsync()
    {
        return m_vsync;
    }
    public void SetVsync(bool value)
    {
        m_vsync = value;
    }

    private bool m_bloom;
    public bool GetBloom()
    {
        return m_bloom;
    }
    public void SetBloom(bool value)
    {
        m_bloom = value;
    }

    private bool m_motionBlur;
    public bool GetMotionBlur()
    {
        return m_motionBlur;
    }
    public void SetMotionBlur(bool value)
    {
        m_motionBlur = value;
    }

    private float m_volume;
    public float GetVolume()
    {
        return m_volume;
    }
    public void SetVolume(float value)
    {
        m_volume = value;
    }

    private int m_frameRate;
    public string GetFrameRate()
    {
        return m_frameRate.ToString();
    }
    public void SetFrameRate(string value)
    {
        m_frameRate = int.Parse(value);
    }

    private ShadowQualityLevels m_shadowQuality;
    public string GetShadowQuality()
    {
        return m_shadowQuality.ToString();
    }
    public void SetShadowQuality(string value)
    {
        m_shadowQuality = (ShadowQualityLevels)Enum.Parse(typeof(ShadowQualityLevels), value);
    }

    private Resolution m_resolution;
    public string GetResolution()
    {
        return m_resolution.width + " x " + m_resolution.height;
    }
    public void SetResolution(string val) // string in the form of "1920 x 1080"
    {
        string[] split = val.Split('x');
        m_resolution.width = int.Parse(split[0].Trim());
        m_resolution.height = int.Parse(split[1].Trim());
    }

    public static string[] GetSupportedResolutions()
    {
        List<string> resolutions = new List<string>();
        foreach (Resolution res in Screen.resolutions)
        {
            string resolution = res.width + " x " + res.height;
            if (!resolutions.Contains(resolution))
            {
                resolutions.Add(resolution);
            }
        }
        resolutions.Reverse();
        return resolutions.ToArray();
    }


    private static Settings m_instance = new Settings();
    public static Settings Instance
    {
        get { return m_instance; }
        set { m_instance = value; }
    }

    public Settings()
    {
        LoadDefaults();
    }

	public void Apply()
    {
        Application.targetFrameRate = m_frameRate;
        QualitySettings.vSyncCount = (m_vsync ? 1 : 0);
        QualitySettings.antiAliasing = (m_antialiasing ? 4 : 0);

        if (m_resolution.width != Screen.currentResolution.width || m_resolution.height != Screen.currentResolution.height || m_fullscreen != Screen.fullScreen)
        {
            Screen.SetResolution(m_resolution.width, m_resolution.height, m_fullscreen, m_frameRate);
        }

        switch (m_shadowQuality)
        {
            case ShadowQualityLevels.OFF:
                QualitySettings.shadows = ShadowQuality.Disable;
                break;
            case ShadowQualityLevels.LOW:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowCascades = 2;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                break;
            case ShadowQualityLevels.HIGH:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowCascades = 4;
                QualitySettings.shadowResolution = ShadowResolution.High;
                break;
        }

        AudioListener.volume = m_volume;
    }

    public void LoadDefaults()
    {
        m_fullscreen        = DEF_FULLSCREEN;
        m_showFPS           = DEF_SHOW_FPS;
        m_antialiasing      = DEF_AA;
        m_vsync             = DEF_VSYNC;
        m_bloom             = DEF_BLOOM;
        m_motionBlur        = DEF_MOTION_BLUR;
        m_frameRate         = DEF_FRAMERATE;
        m_volume            = DEF_VOLUME;
        m_shadowQuality     = DEF_SHADOW_QUALITY;

        m_resolution = Screen.resolutions[Screen.resolutions.Length - 1];
    }

    public void Save()
    {
        FileIO.WriteSettings(this);
    }
	
	public static Settings Load()
    {
        return FileIO.ReadSettings();
    }
}
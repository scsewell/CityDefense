using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using InputController;
using Newtonsoft.Json;

/*
 * Stores and maintains user constrols.
 */ 
public class Controls
{
    private Dictionary<GameButton, BufferedButton> m_buttons = new Dictionary<GameButton, BufferedButton>();
    public Dictionary<GameButton, BufferedButton> Buttons
    {
        get { return m_buttons; }
    }
    
    private Dictionary<GameAxis, BufferedAxis> m_axes = new Dictionary<GameAxis, BufferedAxis>();
    public Dictionary<GameAxis, BufferedAxis> Axes
    {
        get { return m_axes; }
    }

    [JsonIgnore]
    private bool m_isMuted = false;
    public bool IsMuted
    {
        get { return m_isMuted; }
        set { m_isMuted = value; }
    }

    public enum RebindState { None, Button, Axis, KeyAxis, ButtonAxis }
    [JsonIgnore]
    private RebindState m_rebindState = RebindState.None;
    public RebindState rebindState
    {
        get { return m_rebindState; }
    }

    [JsonIgnore] private BufferedButton m_rebindingButton = null;
    [JsonIgnore] private BufferedAxis m_rebindingAxis = null;
    [JsonIgnore] private List<KeyCode> m_rebindingPreviousKeys = new List<KeyCode>();
    [JsonIgnore] private List<GamepadButton> m_rebindingPreviousButtons = new List<GamepadButton>();
    [JsonIgnore] private KeyCode m_rebindingAxisKey;
    [JsonIgnore] private GamepadButton m_rebindingAxisButton;
    [JsonIgnore] private Action m_onRebindComplete = null;


    private static Controls m_instance = new Controls();
    public static Controls Instance
    {
        get { return m_instance; }
        set { m_instance = value; }
    }

    public Controls()
    {
        LoadDefaults();
    }

    /*
     * Needs to run at the end of every FixedUpdate frame to handle the input buffers.
     */
    public void FixedUpdate()
    {
        foreach (KeyValuePair<GameButton, BufferedButton> button in m_buttons)
        {
            button.Value.RecordFixedUpdateState();
        }
        foreach (KeyValuePair<GameAxis, BufferedAxis> axis in m_axes)
        {
            axis.Value.RecordFixedUpdateState();
        }
    }

    /*
     * Needs to run at the start of every Update frame to buffer new inputs.
     */
    public void EarlyUpdate()
    {
        foreach (KeyValuePair<GameButton, BufferedButton> button in m_buttons)
        {
            button.Value.RecordUpdateState();
        }
        foreach (KeyValuePair<GameAxis, BufferedAxis> axis in m_axes)
        {
            axis.Value.RecordUpdateState();
        }
    }

    /*
     * When rebinding detects any appropriate inputs.
     */
    public void Update()
    {
        if (m_rebindState != RebindState.None)
        {
            if (m_rebindState == RebindState.Axis)
            {
                ISource<float> source = GetAxisSource();
                if (source != null)
                {
                    m_rebindState = RebindState.None;
                    m_rebindingAxis.AddSource(source);
                    m_onRebindComplete();
                }
                else
                {
                    List<KeyCode> activeKeys = FindActiveKeys(true);
                    List<GamepadButton> activeButtons = FindActiveButtons(true);
                    if (activeButtons.Count > 0)
                    {
                        m_rebindState = RebindState.ButtonAxis;
                        m_rebindingAxisButton = activeButtons.First();
                    }
                    else if (activeKeys.Count > 0)
                    {
                        m_rebindState = RebindState.KeyAxis;
                        m_rebindingAxisKey = activeKeys.First();
                    }
                }
            }
            else if (m_rebindState == RebindState.Button)
            {
                ISource<bool> source = GetButtonSource();
                if (source != null)
                {
                    m_rebindState = RebindState.None;
                    m_rebindingButton.AddSource(source);
                    m_onRebindComplete();
                }
            }
            else if (m_rebindState == RebindState.ButtonAxis)
            {
                List<GamepadButton> activeButtons = FindActiveButtons(true);
                if (activeButtons.Count > 0)
                {
                    m_rebindState = RebindState.None;
                    m_rebindingAxis.AddSource(new JoystickButtonAxis(activeButtons.First(), m_rebindingAxisButton));
                    m_onRebindComplete();
                }
            }
            else if (m_rebindState == RebindState.KeyAxis)
            {
                List<KeyCode> activeKeys = FindActiveKeys(true);
                if (activeKeys.Count > 0)
                {
                    m_rebindState = RebindState.None;
                    m_rebindingAxis.AddSource(new KeyAxis(activeKeys.First(), m_rebindingAxisKey));
                    m_onRebindComplete();
                }
            }

            m_rebindingPreviousKeys = FindActiveKeys(false);
            m_rebindingPreviousButtons = FindActiveButtons(false);
        }
    }

    /*
     * Clears the current controls and replaces them with the default set.
     */
    public void LoadDefaults()
    {
        m_buttons = new Dictionary<GameButton, BufferedButton>();
        
        m_buttons.Add(GameButton.Menu, new BufferedButton(false, new List<ISource<bool>>
        {
            new KeyButton(KeyCode.Escape),
            new JoystickButton(GamepadButton.Start),
        }));
        m_buttons.Add(GameButton.Fire1, new BufferedButton(true, new List<ISource<bool>>
        {
            new KeyButton(KeyCode.Space),
            new JoystickButton(GamepadButton.RTrigger),
        }));


        m_axes = new Dictionary<GameAxis, BufferedAxis>();

        m_axes.Add(GameAxis.TrackX, new BufferedAxis(true, 1f, new List<ISource<float>>
        {
            new KeyAxis(KeyCode.A, KeyCode.D),
            new JoystickAxis(GamepadAxis.LStickX),
        }));
        m_axes.Add(GameAxis.TrackY, new BufferedAxis(true, 1f, new List<ISource<float>>
        {
            new KeyAxis(KeyCode.S, KeyCode.W),
            new JoystickAxis(GamepadAxis.LStickY),
        }));
    }

    /*
     * Returns true if any of the relevant keyboard or joystick buttons are held down.
     */
    public bool IsDown(GameButton button)
    {
        BufferedButton bufferedButton = m_buttons[button];
        return !(m_isMuted && bufferedButton.CanBeMuted) && bufferedButton.IsDown();
    }

    /*
     * Returns true if a relevant keyboard or joystick key was pressed since the last appropriate update.
     */
    public bool JustDown(GameButton button)
    {
        BufferedButton bufferedButton = m_buttons[button];
        bool isFixed = (Time.deltaTime == Time.fixedDeltaTime);
        return !(m_isMuted && bufferedButton.CanBeMuted) && (isFixed ? bufferedButton.JustDown() : bufferedButton.VisualJustDown());
    }

    /*
     * Returns true if a relevant keyboard or joystick key was released since the last appropriate update.
     */
    public bool JustUp(GameButton button)
    {
        BufferedButton bufferedButton = m_buttons[button];
        bool isFixed = (Time.deltaTime == Time.fixedDeltaTime);
        return !(m_isMuted && bufferedButton.CanBeMuted) && (isFixed ? bufferedButton.JustUp() : bufferedButton.VisualJustUp());
    }

    /*
     * Returns the average value of an axis from all Update frames since the last FixedUpdate.
     */
    public float AverageValue(GameAxis axis)
    {
        BufferedAxis bufferedAxis = m_axes[axis];
        return (m_isMuted && bufferedAxis.CanBeMuted) ? 0 : bufferedAxis.GetValue(true);
    }

    /*
     * Returns the cumulative value of an axis from all Update frames since the last FixedUpdate.
     */
    public float CumulativeValue(GameAxis axis)
    {
        BufferedAxis bufferedAxis = m_axes[axis];
        return (m_isMuted && bufferedAxis.CanBeMuted) ? 0 : bufferedAxis.GetValue(false);
    }

    
    public void AddBinding(BufferedButton button, Action onRebindComplete)
    {
        if (m_rebindState == RebindState.None)
        {
            m_rebindState = RebindState.Button;
            m_rebindingButton = button;

            m_onRebindComplete = onRebindComplete;
            m_rebindingPreviousKeys = FindActiveKeys(false);
            m_rebindingPreviousButtons = FindActiveButtons(false);
        }
    }

    public void AddBinding(BufferedAxis axis, Action onRebindComplete)
    {
        if (m_rebindState == RebindState.None)
        {
            m_rebindState = RebindState.Axis;
            m_rebindingAxis = axis;

            m_onRebindComplete = onRebindComplete;
            m_rebindingPreviousKeys = FindActiveKeys(false);
            m_rebindingPreviousButtons = FindActiveButtons(false);
        }
    }

    private ISource<bool> GetButtonSource()
    {
        List<GamepadButton> activeButtons = FindActiveButtons(true);
        if (activeButtons.Count > 0)
        {
            return new JoystickButton(activeButtons.First());
        }
        List<KeyCode> activeKeys = FindActiveKeys(true);
        if (activeKeys.Count > 0)
        {
            return new KeyButton(activeKeys.First());
        }
        return null;
    }
    
    private ISource<float> GetAxisSource()
    {
        foreach (GamepadAxis axis in Enum.GetValues(typeof(GamepadAxis)))
        {
            if (JoystickAxis.GetAxisValue(axis) > 0.5f)
            {
                return new JoystickAxis(axis);
            }
        }
        foreach (MouseAxis.Axis axis in Enum.GetValues(typeof(MouseAxis.Axis)))
        {
            if (MouseAxis.GetAxisValue(axis) > 0.5f)
            {
                return new MouseAxis(axis);
            }
        }
        return null;
    }

    private List<KeyCode> FindActiveKeys(bool ignorePrevious)
    {
        return Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(
            button => KeyButton.GetButtonValue(button) && (!ignorePrevious || !m_rebindingPreviousKeys.Contains(button))
            ).ToList();
    }

    private List<GamepadButton> FindActiveButtons(bool ignorePrevious)
    {
        return Enum.GetValues(typeof(GamepadButton)).Cast<GamepadButton>().Where(
            button => JoystickButton.GetButtonValue(button) && (!ignorePrevious || !m_rebindingPreviousButtons.Contains(button))
            ).ToList();
    }

    public void Save()
    {
        FileIO.WriteControls(Instance);
    }

    public void Load()
    {
        m_instance = FileIO.ReadControls();
    }
}
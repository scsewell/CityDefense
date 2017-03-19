using UnityEngine;
using UnityEngine.UI;

public class UI : Singleton<UI>
{
    [SerializeField]
    private Canvas m_gameUICanvas;

    [SerializeField]
    private Crosshair m_crosshairPrefab;

    private Transform m_crosshairs;
    private Transform m_healthbars;
    
    private enum Menu { None, Root, Settings, Controls, Bindings }
    private Menu m_menu;

    private void Awake()
    {
        m_crosshairs = new GameObject("Crosshairs").transform;
        m_crosshairs.SetParent(m_gameUICanvas.transform, false);

        m_healthbars = new GameObject("Healthbars").transform;
        m_healthbars.SetParent(m_gameUICanvas.transform, false);
    }

    private void Start()
    {
        SetMenu(Menu.None);
    }

    private void Update()
    {
        // show the cursor if a menu is open
        Cursor.lockState = IsMenuOpen() ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = IsMenuOpen();

        // prevent unwanted user input when menu is open
        Controls.Instance.IsMuted = IsMenuOpen();

        // toggle the menu if the menu button was hit, or if the cancel button was hit go back a menu
        if (Controls.Instance.rebindState == Controls.RebindState.None)
        {
            if (Controls.Instance.JustDown(GameButton.Menu))
            {
                SetMenu(IsMenuOpen() ? Menu.None : Menu.Root);
            }
            else if (IsMenuOpen() && Input.GetButtonDown("Cancel"))
            {
                switch (m_menu)
                {
                    case Menu.Root: SetMenu(Menu.None); break;
                    case Menu.Settings:
                    case Menu.Controls: SetMenu(Menu.Root); break;
                    case Menu.Bindings: SetMenu(Menu.Controls); break;
                }
            }
        }
    }

    public Crosshair AddCrosshair()
    {
        return Instantiate(m_crosshairPrefab, m_crosshairs, false);
    }

    public Healthbar AddHealthbar(Healthbar healthbar)
    {
        return Instantiate(healthbar, m_healthbars, false);
    }

    private void SetMenu(Menu menu)
    {
        m_gameUICanvas.enabled = (menu == Menu.None);
        m_menu = menu;

        if (m_menu == Menu.None)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public bool IsMenuOpen()
    {
        return m_menu != Menu.None;
    }
}

/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region variables
    public static GameManager instance;

    float GameSpeed = 1f;
    float DeltaTime;

    bool ShowCombatDamage = true;

    public OpenSpaceProtected.ExploderObject exploder;

    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public LayerMask AlliedLayer;
	public LayerMask NeutralLayer;

    public RenderTexture miniMapTexture;

    public GUIStyle standardBackGround;

    public GUIStyle standardButtonStyle;
    public GUIStyle SquareButtonGreenStyle;
    public GUIStyle standardLabelStyle;
    public GUIStyle largeLabelStyle;
    public GUIStyle ToolTipTitleStyle;
    public GUIStyle ToolTipBodyStyle;

    public GUIStyle ModuleTitleStyle;
    public GUIStyle ModuleDescStyle;
    public GUIStyle ModuleModStyle;

    public GUIStyle StatBarStyle;

    public GUIStyle CreditsTitleStyle;
    public GUIStyle CreditsEntryStyle;

    public GUISkin standardSkin;

    [HideInInspector]
    public GUIContent UIContent; 

    //Standardized UI sizes
    [HideInInspector]
    public Vector2 StandardButtonSize;
    [HideInInspector]
    public Vector2 StandardLabelSize;
    [HideInInspector]
    public float QuarterButtonSpacing;
    [HideInInspector]
    public float standardLabelSpacing;
    [HideInInspector]
    public float QuarterLabelSpacing;

    // Screen varables
    ScreenParent currentScreen;
    ScreenParent nextScreen;
    float screenTransitionTimer;
    bool FadeOut = false;
    bool FadeIn = false;

    [HideInInspector]
    public Quaternion startingCameraRotation;
    #endregion

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        exploder = GameObject.Find("Exploder").GetComponent<OpenSpaceProtected.ExploderObject>();
        startingCameraRotation = Camera.main.transform.rotation;
        SetScreenElementSizes();
        ChangeScreen(new MainMenuScreen());
    }
	
	// Update is called once per frame
	void Update ()
    {
        DeltaTime = Time.deltaTime * GameSpeed;

        if (screenTransitionTimer > 0)
        {
            screenTransitionTimer -= Time.deltaTime * ResourceManager.gameConstants.ScreenTransitionRate;
            if (screenTransitionTimer <= 0)
            {
                screenTransitionTimer = 0;
                FadeIn = false;
                if (FadeOut)
                {
                    TransitionScreen();
                }
            }
        }

        if (currentScreen != null)
        {
            currentScreen.Update();
        }
    }

    void OnGUI()
    {
        GUI.skin = standardSkin;

        if (FadeOut)
        {
            GUI.color = new Color(1f, 1f, 1f, screenTransitionTimer);
        }
        else if (FadeIn)
        {
            GUI.color = new Color(1f, 1f, 1f, 1f - screenTransitionTimer);
        }

        if (currentScreen != null)
        {
            currentScreen.Draw();
        }
    }

    public void ChangeScreen(ScreenParent newScreen)
    {
        nextScreen = newScreen;
        screenTransitionTimer = 1f;
        FadeOut = true;
    }

    public void TransitionScreen()
    {
        screenTransitionTimer = 1f;
        FadeOut = false;
        FadeIn = true;
        currentScreen = nextScreen;
        nextScreen = null;
    }

    void SetScreenElementSizes()
    {
        StandardButtonSize = new Vector2(Screen.width * 0.07f, Screen.height * 0.033f);
        StandardLabelSize = new Vector2(Screen.width * 0.05f, Screen.height * 0.028f);
        standardLabelSpacing = StandardLabelSize.y * 0.5f;
        QuarterLabelSpacing = StandardLabelSize.y * 0.25f;
        QuarterButtonSpacing = StandardButtonSize.y * 0.25f;
    }

	public void CreateOutLineSystems(Color PlayerColor, Color EnemyColor, Color AlliedColor, Color NeutralColor)
    {
        OpenSpaceProtected.OutlineSystem ObjectOutlinerPlayer = ResourceManager.CreateOutLineSystem();
        ObjectOutlinerPlayer.outlineLayer = PlayerLayer;
        ObjectOutlinerPlayer.outlineColor = PlayerColor;
        OpenSpaceProtected.OutlineSystem ObjectOutlinerEnemy = ResourceManager.CreateOutLineSystem();
        ObjectOutlinerEnemy.outlineLayer = EnemyLayer;
        ObjectOutlinerEnemy.outlineColor = EnemyColor;
        OpenSpaceProtected.OutlineSystem ObjectOutlinerAllied = ResourceManager.CreateOutLineSystem();
        ObjectOutlinerAllied.outlineLayer = AlliedLayer;
        ObjectOutlinerAllied.outlineColor = AlliedColor;
        OpenSpaceProtected.OutlineSystem ObjectOutlinerNeutral = ResourceManager.CreateOutLineSystem();
		ObjectOutlinerNeutral.outlineLayer = NeutralLayer;
		ObjectOutlinerNeutral.outlineColor = NeutralColor;
    }

    public void SetGameSpeed(float speed)
    {
        GameSpeed = speed;
    }

    public float GetDeltaTime()
    {
        return DeltaTime;
    }

    public float GetGameSpeed()
    {
        return GameSpeed;
    }

    public bool GetShowCombatDamage()
    {
        return ShowCombatDamage;
    }

    public void SetShowCombatDamage(bool state)
    {
        ShowCombatDamage = state;
    }
}

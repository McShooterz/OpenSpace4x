/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Base class for all controllable units in space battles
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public abstract class SpaceUnit : MonoBehaviour 
{
    #region Variables
    protected ShipManager shipManager;
    [HideInInspector]
    public GameObject unitMeshObject;

    //Components
    protected AudioSource audioSource;
	protected OpenSpaceProtected.Forcefield forceField;
	protected ShipOrderLine shipOrderLine;
    protected GameObject MiniMapObject;
    protected Material currentMaterial;
    protected TextMeshController textMeshController;
    protected BoardingBalanceBar boardingBalanceBar;

    //States
    protected bool isPaused = true;
	protected bool Disabled = false;
	protected bool Destroyed = false;
	protected int HighlightLayer = 0;
	protected bool ActiveHighlight = false;
	protected bool HoveredHighlight = false;

	//AI
	protected SpaceUnit Target;
	protected bool TargetIsEnemy = false;
    protected float WeaponCheckTimer = 0;
    protected float MainTargetRange;
    protected Vector2 MainTargetDirection;
    protected bool hasPointDefense;
    protected List<TargetInfo> CachedTargets = new List<TargetInfo>();
    protected List<PDTargetInfo> PDCachedTargets = new List<PDTargetInfo>();
    #endregion
    // Use this for initialization
    protected virtual void Start () 
	{

	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
        if (Destroyed)
            return;
        HighlightUpdate();
    }

	protected void HighlightUpdate()
	{
		if (ActiveHighlight || HoveredHighlight) {
			transform.GetChild (0).gameObject.layer = HighlightLayer;
			HoveredHighlight = false;
		} else {
			transform.GetChild(0).gameObject.layer = 0;
		}
	}

	public virtual void Initialize()
	{
		forceField = GetComponentInChildren<OpenSpaceProtected.Forcefield>();
	}

	public void SetShipManager(ShipManager SM)
	{
		shipManager = SM;
	}

	public ShipManager GetShipManager()
	{
		return shipManager;
	}

	public void SetAudioSource(AudioSource AS)
	{
		audioSource = AS;
	}

	public void SetHighLightLayer(int layer)
	{
		HighlightLayer = layer;
	}

	public void AddOrderLine()
	{
		GameObject OrderLineObject = ResourceManager.CreateShipOrderLine();
		OrderLineObject.transform.position = transform.position;
		OrderLineObject.transform.parent = transform;
		shipOrderLine = OrderLineObject.GetComponent<ShipOrderLine>();
	}

    public void RemoveOrderLine()
    {
        if (shipOrderLine != null)
        {
            Destroy(shipOrderLine.gameObject);
            shipOrderLine = null;
        }
    }

	public Vector3 GetPosition()
	{
		return transform.position;
	}

	public virtual void SetPause(bool state)
	{
		isPaused = state;
	}

	public void ToggleHighlight(bool state)
	{
		ActiveHighlight = state;
	}

	public void Hovered()
	{
		HoveredHighlight = true;
	}

	public virtual void ActivateOrdersLine()
	{
		if (shipOrderLine == null)
			return;
		if(Target != null)
		{
			if(TargetIsEnemy)
			{
				shipOrderLine.TurnOn(1.0f, ResourceManager.gameConstants.Highlight_Enemy.GetColor(), Target);
			}
			else
			{
				shipOrderLine.TurnOn(1.0f, ResourceManager.gameConstants.Highlight_Player.GetColor(), Target);
			}
		}
	}

	public virtual void SetNewTarget(SpaceUnit target, bool Enemy)
	{
        if (target != null)
        {
            Target = target;
            TargetIsEnemy = Enemy;
            if (shipOrderLine != null)
            {
                if (Enemy)
                    shipOrderLine.TurnOn(1.0f, ResourceManager.gameConstants.Highlight_Enemy.GetColor(), target);
                else
                    shipOrderLine.TurnOn(1.0f, ResourceManager.gameConstants.Highlight_Player.GetColor(), target);
            }
        }
	}

	public virtual void CancelOrders()
	{
		Target = null;
	}

	public bool isEnemy(SpaceUnit target)
	{
		if(shipManager != null)
			return shipManager.isEnemy(target);
		else
			return false;
	}

    public virtual void CheckShieldHit(Vector3 point, Vector2 origin)
    {

    }

	public void ShieldHit(Vector3 point)
	{
		forceField.OnHit(point, 20f);
	}

	public SpaceUnit GetTarget()
	{
		return Target;
	}

    public bool CanBeTargeted()
    {
        if (Destroyed)
            return false;
        if(this is Ship)
        {
            if ((this as Ship).IsCloaked())
                return false;
        }
        return true;
    }

    public void AttachMiniMapObject()
    {
        if (MiniMapObject == null)
        {
            MiniMapObject = ResourceManager.CreateMiniMapObject();
            MiniMapObject.transform.position = gameObject.transform.position;
            MiniMapObject.transform.parent = gameObject.transform;
        }
        SetMiniMapObjectColor(shipManager.GetHighlightColor());
    }

    public void SetMiniMapObjectColor(Color color)
    {
        if(MiniMapObject != null)
        {
            MiniMapObject.GetComponent<Renderer>().material.color = color;
        }
    }

    public void PlaySound(string clip)
    {
        AudioManager.instance.PlayEffectClip(audioSource, clip, true);
    }

    protected float GetDeltaTime()
    {
        return GameManager.instance.GetDeltaTime();
    }

    public bool isDestroyed()
    {
        return Destroyed;
    }

    protected bool inRange(Vector3 Target, float Range)
    {
        float SqrMagnitude = (Target - transform.position).sqrMagnitude;
        if (SqrMagnitude < Range)
        {
            return true;
        }
        return false;
    }

    protected virtual float GetMaxWeaponRangeSqr()
    {
        return 0;
    }

    protected void CacheTargets()
    {
        if (Target != null && TargetIsEnemy)
        {
            MainTargetDirection = new Vector2(Target.transform.position.x - transform.position.x, Target.transform.position.z - transform.position.z);
            MainTargetRange = MainTargetDirection.sqrMagnitude;
        }

        CachedTargets = shipManager.GetPotentialTargets(new Vector2(transform.position.x, transform.position.z), GetMaxWeaponRangeSqr());
        if(hasPointDefense)
        {
            PDCachedTargets = shipManager.GetPotentialPDTargets(new Vector2(transform.position.x, transform.position.z), GetMaxWeaponRangeSqr());
        }
    }

    public void AttachFireDamage(Vector3 Position, float Scale)
    {
        GameObject damageFire = ResourceManager.CreateShipFireDamage(Scale);
        damageFire.transform.position = unitMeshObject.transform.TransformPoint(Position);
        damageFire.transform.rotation = Quaternion.Euler(Random.Range(260f, 280f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        damageFire.transform.parent = unitMeshObject.transform;
    }

    protected QuadrantTypes GetQuadrantDirection(Vector2 direction)
    {
        Vector2 forwardRotation = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 portRotation = new Vector2(-transform.right.x, -transform.right.z);

        if (Vector2.Angle(forwardRotation, direction) <= 45f)
        {
            return QuadrantTypes.Fore;
        }
        else if (Vector2.Angle(-forwardRotation, direction) <= 45f)
        {
            return QuadrantTypes.Aft;
        }
        else if (Vector2.Angle(portRotation, direction) <= 45f)
        {
            return QuadrantTypes.Port;
        }
        else
        {
            return QuadrantTypes.Starboard;
        }
    }

    public void CreateShieldDamagePopup(float value)
    {
        ResourceManager.CreatePopupMessage(transform.position, value.ToString("0"), Color.cyan, 3f);
    }

    public void CreateArmorDamagePopup(float value)
    {
        ResourceManager.CreatePopupMessage(transform.position, value.ToString("0"), new Color(0.75f,0.75f,0.75f,1f), 3f);
    }

    public void CreateHealthDamagePopup(float value)
    {
        ResourceManager.CreatePopupMessage(transform.position, value.ToString("0"), Color.red, 3f);
    }

    public void CreateShieldDamagePopup(Vector3 position, float value)
    {
        ResourceManager.CreatePopupMessage(position, value.ToString("0"), Color.cyan, 3f);
    }

    public void CreateArmorDamagePopup(Vector3 position, float value)
    {
        ResourceManager.CreatePopupMessage(position, value.ToString("0"), new Color(0.66f, 0.66f, 0.66f, 1f), 3f);
    }

    public void CreateHealthDamagePopup(Vector3 position, float value)
    {
        ResourceManager.CreatePopupMessage(position, value.ToString("0"), Color.red, 3f);
    }

    protected void AttachTextMeshController(string message, Color color)
    {
        if (textMeshController != null)
            Destroy(textMeshController.gameObject);
        textMeshController = ResourceManager.CreateWorldMessage(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), message, color, 36);
        textMeshController.transform.parent = transform;
    }

    protected void AttachBoardingBalanceBar()
    {
        if (boardingBalanceBar != null)
            Destroy(boardingBalanceBar.gameObject);
        boardingBalanceBar = ResourceManager.CreateBoardingBalanceBar(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
        boardingBalanceBar.transform.parent = transform;
    }

    public void SetBoardingForceBalance(float value)
    {
        if (boardingBalanceBar == null)
            AttachBoardingBalanceBar();
        boardingBalanceBar.SetGradient(value);
    }

    public void RemoveBoardingBalanceBar()
    {
        Destroy(boardingBalanceBar.gameObject);
    }

    public virtual bool HasEnoughPowerAndAmmo(float power, float ammo)
    {
        return true;
    }

    public virtual bool CanGetAmmo(float ammo)
    {
        return true;
    }

    public virtual float GetDamageBonus()
    {
        return 0;
    }

    public virtual void ConsumePowerAndAmmo(float power, float ammo)
    {

    }

    public virtual bool ConsumePower(float power)
    {
        return true;
    }

    public virtual void GetKill(SpaceUnit killedUnit)
    {

    }

    public virtual void RecordDamage(float amount)
    {

    }

    public virtual float GetWeaponDelayModifier()
    {
        return 0;
    }
}

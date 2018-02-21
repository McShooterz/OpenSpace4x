/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class MobileSpaceUnit : SpaceUnit 
{
	//States
	protected bool Mobile = true;

	//AI
	protected Vector3 goalPosition = Vector3.zero;
	protected bool ValidMoveOrder = false;
	protected float attackRange;

	//Movement
	protected Vector3 Velocity = Vector3.zero;
	protected float acceleration = 0;

    // Use this for initialization
    protected override void Start () 
	{
	
	}

    // Update is called once per frame
    protected override void Update () 
	{
        base.Update();
	}

	public override void Initialize()
	{
		base.Initialize ();
	}

	public override void SetPause(bool state)
	{
		base.SetPause (state);
	}

    public void SetAttackRange(float range)
    {
        attackRange = range;
    }

	protected void UpdateVelocity()
	{
		transform.position += Velocity * GetDeltaTime();

		//Add Drag
		Velocity *= ResourceManager.instance.GetGameConstants().ShipDrag;
	}

	public void ApplyBrake()
	{
		Velocity *= ResourceManager.instance.GetGameConstants().ShipBrakeDrag;
	}

	public override void ActivateOrdersLine()
	{
		if (shipOrderLine == null)
			return;
		if(ValidMoveOrder)
		{
			shipOrderLine.TurnOn(1.0f, ResourceManager.instance.GetGameConstants().Highlight_Player.GetColor(), goalPosition);
		}
		else if(Target != null)
		{
			if(TargetIsEnemy)
			{
				shipOrderLine.TurnOn(1.0f, ResourceManager.instance.GetGameConstants().Highlight_Enemy.GetColor(), Target);
			}
			else
			{
				shipOrderLine.TurnOn(1.0f, ResourceManager.instance.GetGameConstants().Highlight_Player.GetColor(), Target);
			}
		}
	}

	public override void SetNewTarget(SpaceUnit target, bool Enemy)
	{
		base.SetNewTarget (target, Enemy);
		ValidMoveOrder = false;
	}

	public virtual void SetNewGoalPosition(Vector3 Position)
	{
		Target = null;
		ValidMoveOrder = true;
		if(shipOrderLine != null)
		{
			shipOrderLine.TurnOn(1.0f, ResourceManager.instance.GetGameConstants().Highlight_Player.GetColor(), Position);
        }
	}

	public void SetGoalPosition(Vector3 Position)
	{
		goalPosition = Position;
	}

	public override void CancelOrders()
	{
		base.CancelOrders ();
		ValidMoveOrder = false;
	}

	protected virtual bool MoveTowardsGoal(Vector3 goal)
	{
		Vector3 Direction = (goal - transform.position).normalized;
		float Angle = Vector3.Angle(transform.forward, Direction);
		float goalDistance = Vector3.SqrMagnitude(goal - transform.position);

		//Check to see if ship is moving in wrong direction, and should apply brake
		float MovementAngle = Vector3.Angle(Direction, Velocity);
		if (MovementAngle > 60f) 
		{
			ApplyBrake ();
		}

		//Check to see if should move forward
		if (goalDistance > 1f)
		{
			RotateTowards(Direction);
			if (Angle < 20f)
			{
				MoveForwards();
			}
            return false;
		}
		else
		{
			ValidMoveOrder = false;
            return true;
		}
	}

	protected void RotateTowards(Vector3 direction)
	{
		if (direction.sqrMagnitude > 0)
		{
			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation =  Quaternion.Lerp(transform.rotation, rotation, GetTurnRate() * GetDeltaTime());
		}
	}

    protected virtual void RotateTowards(Quaternion Rotation)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, GetTurnRate() * GetDeltaTime());
    }

    protected void MoveForwards()
	{
		if(Velocity.sqrMagnitude < Mathf.Pow(GetMaxSpeed(), 2f))
			Velocity += transform.forward * acceleration;
	}

	protected virtual float GetMaxSpeed()
	{
		return 0f;
	}

	protected virtual float GetTurnRate()
	{
		return 0f;
	}

    public Vector3 GetVelocity()
    {
        return Velocity;
    }
}

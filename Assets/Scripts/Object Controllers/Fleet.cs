/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Attaches to fleet game object to define it's behavior on the campaign map
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class Fleet : MonoBehaviour
{
    //Variables
    public FleetData fleetData;

    GameObject Target;
    LineRenderer GoalLine;
    GameObject ShipMesh;

    float GoalLineOffset;

    bool ValidGoalPosition = false;
    bool ValidTarget = false;
    bool Selected;


    // Use this for initialization
    void Start ()
    {
        GoalLine = GetComponent<LineRenderer>();
        //ShipMesh = AddShipMesh("Galaxy Class");
        Highlight(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Animate goal line if fleet is selected
	    if(Selected)
        {
            GoalLineOffset -= 2f * Time.deltaTime;
            GoalLine.material.SetTextureOffset("_MainTex", new Vector2(GoalLineOffset, 0));
        }
	}

    public void SetGoalPosition(Vector3 goal)
    {
        Vector3 Direction = (goal - transform.position).normalized;

        ValidGoalPosition = true;
        ValidTarget = false;

        //Set goal line
        GoalLine.SetPosition(0, transform.position);
        GoalLine.SetPosition(1, goal);

        //Set scaling for goal line
        GoalLine.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position,goal),1);

        //Rotate towards goal
        transform.rotation = Quaternion.LookRotation(Direction);
    }

    //Turning on and off being in selected state visually
    public void Highlight(bool isSelected)
    {
        GoalLine.enabled = isSelected;      
        Selected = isSelected;
        if (isSelected)
        {
            ShipMesh.GetComponentInChildren<Renderer>().material.SetFloat("_OutlinePower", 1.0f);
        }
        else
        {
            ShipMesh.GetComponentInChildren<Renderer>().material.SetFloat("_OutlinePower", 0.0f);
        }
    }

    public void SetHighlightColor(Color color)
    {
        ShipMesh.GetComponentInChildren<Renderer>().material.SetColor("_OutlineColor", color);
    }

    public GameObject AddShipMesh(ShipHullData hullData)
    {
        Bounds bounds;
        float MaxDimension;

        GameObject ship = ResourceManager.CreateShip(hullData, transform.position, transform.rotation);
        //Assign as child
        ship.transform.parent = transform;
        //Get size of ship for scaling
        bounds = ship.GetComponentInChildren<MeshFilter>().mesh.bounds;
        //bounds = ship.GetComponent<MeshFilter>().mesh.bounds;
        MaxDimension = Mathf.Max(bounds.size.x, bounds.size.z);
        //Scale to a particular size no matter original size
        ship.transform.localScale = new Vector3(2.3f / MaxDimension, 2.1f / MaxDimension, 2.1f / MaxDimension);
        return ship;
    }

    public void RemoveShipMesh()
    {
        Destroy(ShipMesh);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

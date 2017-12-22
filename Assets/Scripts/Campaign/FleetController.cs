/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Attaches to fleet game object to define it's behavior on the campaign map
******************************************************************************************************************************************/

using UnityEngine;

public class FleetController : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    Vector3 goalPosition;

    [SerializeField]
    LineRenderer goalLine;

    [SerializeField]
    GameObject shipMesh;

    [SerializeField]
    float goalLineOffset;

    [SerializeField]
    bool validGoalPosition = false;

    [SerializeField]
    bool validTarget = false;

    [SerializeField]
    bool selected;

    [SerializeField]
    FleetData fleetData;

    // Use this for initialization
    void Start ()
    {
        Highlight(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Animate goal line if fleet is selected
	    if(selected)
        {
            goalLineOffset -= 2f * Time.deltaTime;
            goalLine.material.SetTextureOffset("_MainTex", new Vector2(goalLineOffset, 0));
        }
	}

    public FleetData GetFleetData()
    {
        return fleetData;
    }

    public void SetFleetData(FleetData data)
    {
        fleetData = data;
    }

    public void SetGoalPosition(Vector3 goal)
    {
        Vector3 Direction = (goal - transform.position).normalized;

        validGoalPosition = true;
        validTarget = false;

        //Set goal line
        goalLine.SetPosition(0, transform.position);
        goalLine.SetPosition(1, goal);

        //Set scaling for goal line
        goalLine.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position,goal),1);

        //Rotate towards goal
        transform.rotation = Quaternion.LookRotation(Direction);
    }

    //Turning on and off being in selected state visually
    public void Highlight(bool isSelected)
    {
        goalLine.enabled = isSelected;      
        selected = isSelected;
        if (isSelected)
        {
            shipMesh.GetComponentInChildren<Renderer>().material.SetFloat("_OutlinePower", 1.0f);
        }
        else
        {
            shipMesh.GetComponentInChildren<Renderer>().material.SetFloat("_OutlinePower", 0.0f);
        }
    }

    public void SetHighlightColor(Color color)
    {
        shipMesh.GetComponentInChildren<Renderer>().material.SetColor("_OutlineColor", color);
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
        Destroy(shipMesh);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

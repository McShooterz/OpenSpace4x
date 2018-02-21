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

    //[SerializeField]


    [SerializeField]
    FleetData fleetData;

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(validGoalPosition && goalLine.enabled)
        {
            goalLine.SetPosition(0, transform.position);
            goalLine.SetPosition(1, goalPosition);

            //Animate goal line if fleet is selected
            goalLineOffset -= 2f * Time.deltaTime;
            goalLine.material.SetTextureOffset("_MainTex", new Vector2(goalLineOffset, 0));
            goalLine.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position, goalPosition), 1);
        }

        if (validGoalPosition)
        {
            float distanceSquare = (goalPosition - transform.position).sqrMagnitude;

            if (distanceSquare > 0.2f)
            {
                moveTowardsPosition(goalPosition);
            }
            else
            {
                validGoalPosition = false;
            }
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

    public void SetGoalPosition(Vector3 position)
    {
        goalPosition = position;
        validGoalPosition = true;
        ClearTarget();
    }

    public void ToggleLineRender(bool state)
    {
        goalLine.enabled = state;
    }

    public void ClearTarget()
    {
        target = null;
        validTarget = false;
    }

    public void ClearGoalPosition()
    {
        goalLine.enabled = false;
        validGoalPosition = false;
    }

    public GameObject AddShipMesh(ShipHullData hullData)
    {
        Bounds bounds;
        float MaxDimension;

        if (shipMesh != null)
        {
            RemoveShipMesh();
        }

        GameObject ship = ResourceManager.instance.CreateShip(hullData, transform.position, transform.rotation);
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

    void moveTowardsPosition(Vector3 position)
    {
        Vector3 direction = position - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);

        transform.position = Vector3.MoveTowards(transform.position, position, fleetData.GetSpeedFTL());
    }
}

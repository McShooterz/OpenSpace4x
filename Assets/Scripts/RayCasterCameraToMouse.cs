/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCasterCameraToMouse : MonoBehaviour
{
    [SerializeField]
    RaycastHit rayCastHit;

    [SerializeField]
    bool validCast = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            validCast = false;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out rayCastHit, Mathf.Infinity))
            {
                validCast = true;
            }
            else
            {
                validCast = false;
            }
        }
    }

    public bool IsValidCast()
    {
        return validCast;
    }

    public RaycastHit GetRayCastHit()
    {
        return rayCastHit;
    }
}

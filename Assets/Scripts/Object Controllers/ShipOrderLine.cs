/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ShipOrderLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    float timer = 0;
    SpaceUnit Target = null;
    Vector3 Position = Vector3.zero;
    Color Color;
    bool objectTarget = false;
	
	// Update is called once per frame
	void Update ()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            Color.a = timer;
            lineRenderer.startColor = Color;
            lineRenderer.endColor = Color;
            lineRenderer.SetPosition(0, transform.position);
            if(objectTarget)
            {
                if(Target != null)
                {
                    lineRenderer.SetPosition(1, Target.transform.position);
                }
                else
                {
                    timer = 0;
                }
            }
            else
            {
                lineRenderer.SetPosition(1, Position);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
	}

    public void TurnOn(float time, Color color, SpaceUnit target)
    {
        gameObject.SetActive(true);
        timer = time;
        Color = color;
        Target = target;
        objectTarget = true;
        lineRenderer.startColor = Color;
        lineRenderer.endColor = Color;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    public void TurnOn(float time, Color color, Vector3 position)
    {
        gameObject.SetActive(true);
        timer = time;
        Color = color;
        Position = position;
        objectTarget = false;
        lineRenderer.startColor = Color;
        lineRenderer.endColor = Color;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }
}

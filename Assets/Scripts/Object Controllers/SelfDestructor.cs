/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class SelfDestructor : MonoBehaviour
{
    public float lifeTimer = 10f;
	
	// Update is called once per frame
	void Update ()
    {
	    if(lifeTimer > 0)
        {
            lifeTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public void setLifeTimer(float time)
    {
        lifeTimer = time;
    }
}

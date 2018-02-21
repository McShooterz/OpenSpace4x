using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustableBarController : MonoBehaviour
{

    [SerializeField]
    Image background;

    [SerializeField]
    Image activeFill;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetPercentage(float percent)
    {
        activeFill.fillAmount = percent;
    }


}

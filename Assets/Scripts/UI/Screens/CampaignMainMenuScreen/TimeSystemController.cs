

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystemController : MonoBehaviour
{
    [SerializeField]
    CampaignMainScreen parentScreen;

    [SerializeField]
    Image playIcon;

    [SerializeField]
    Image pauseIcon;

    [SerializeField]
    Text stardateText;

    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Stardate stardate;

    public static float timeModifier = 1f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


}

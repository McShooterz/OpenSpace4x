/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignManager : MonoBehaviour
{
    public CampaignManager instance;


    //Hides public variables below this from appearing in the inspector
    [HideInInspector]

    public List<Empire> Empires = new List<Empire>();

    public Empire PlayerEmpire;

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        //Create players Empire
        CreateEmpire(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}


    void CreateEmpire(bool isPlayer)
    {
        Empire empire = new Empire();
        empire.isPlayer = isPlayer;
        Empires.Add(empire);
        empire.Initialize();
        PlayerEmpire = empire;
    }
}

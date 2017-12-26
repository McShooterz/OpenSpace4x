using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireManager : MonoBehaviour
{
    public static EmpireManager instance;

    [SerializeField]
    Empire playerEmpire;

    [SerializeField]
    List<Empire> empires = new List<Empire>();

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public Empire GetPlayerEmpire()
    {
        return playerEmpire;
    }

    public List<Empire> GetEmpires()
    {
        return empires;
    }
}

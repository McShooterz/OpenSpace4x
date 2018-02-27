using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoQuick : MonoBehaviour
{
    [SerializeField]
    Text planetName;

    [SerializeField]
    Text planetType;

    [SerializeField]
    Text planetSize;

    [SerializeField]
    Text planetPopulation;

    [SerializeField]
    Text planetMoral;

    [SerializeField]
    Text planetMoney;

    [SerializeField]
    Text planetFood;

    [SerializeField]
    Text planetMetal;

    [SerializeField]
    Text planetCrystal;

    [SerializeField]
    Text planetPhysics;

    [SerializeField]
    Text planetSociety;

    [SerializeField]
    Text planetEngineering;

    [SerializeField]
    Text planetUnity;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetPlanetNameText(string name)
    {
        planetName.text = name;
    }

    public void SetPlanetTypeText(string type)
    {
        planetType.text = type;
    }

    public void SetPlanetSizeText(string size)
    {
        planetSize.text = size;
    }

    public void SetPlanetPopulationText(string population)
    {
        planetPopulation.text = population;
    }

    public void SetPlanetMoralText(string moral)
    {
        planetMoral.text = moral;
    }

    public void SetPlanetMoneyText(string money)
    {
        planetMoney.text = money;
    }

    public void SetPlanetFoodText(string food)
    {
        planetFood.text = food;
    }

    public void SetPlanetMetalText(string metal)
    {
        planetMetal.text = metal;
    }

    public void SetPlanetCrystalText(string crystal)
    {
        planetCrystal.text = crystal;
    }

    public void SetPlanetPhysicsText(string physics)
    {
        planetPhysics.text = physics;
    }

    public void SetPlanetSocietyText(string society)
    {
        planetSociety.text = society;
    }

    public void SetPlanetEngineeringText(string engineering)
    {
        planetEngineering.text = engineering;
    }

    public void SetPlanetUnityText(string unity)
    {
        planetUnity.text = unity;
    }
}

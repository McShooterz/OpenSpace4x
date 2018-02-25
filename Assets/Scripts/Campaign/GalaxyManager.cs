/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GalaxyManager : MonoBehaviour
{
    public static GalaxyManager instance;

    [Header("Galaxy Generation Settings")]

    [SerializeField]
    int galaxySize;

    [SerializeField]
    int systemCount;

    [SerializeField]
    float minSystemDistance;

    [SerializeField]
    int minPlanetsPerSystem;

    [SerializeField]
    int maxPlanetsPerSystem;

    Dictionary<Guid, PlanetData> planetDatas = new Dictionary<Guid, PlanetData>();

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

        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        BuildGalaxy(GetRandomSystemPoints());

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}



    void BuildGalaxy(Vector3[] solarSystemPositions)
    {



        foreach(Vector3 position in solarSystemPositions)
        {
            GameObject starObject = ResourceManager.instance.CreateStar("Star_Red");
            starObject.transform.position = position;
            starObject.transform.rotation = UnityEngine.Random.rotation;

            Vector3[] planetPositions = GetRandomPlanetPositions(UnityEngine.Random.Range(minPlanetsPerSystem, maxPlanetsPerSystem + 1));
            foreach (Vector3 planetPosition in planetPositions)
            {
                GameObject planetObject = ResourceManager.instance.CreatePlanet("Planet_Desert1");
                planetObject.transform.position = planetPosition + position;
                planetObject.transform.rotation = UnityEngine.Random.rotation;
            }
        }
    }


    public void AddPlanetData(Guid guid, PlanetData planetData)
    {
        if (planetDatas.ContainsKey(guid))
        {
            planetDatas[guid] = planetData;
        }
        else
        {
            planetDatas.Add(guid, planetData);
        }
    }

    public PlanetData GetPlanetData(Guid guid)
    {
        PlanetData planetData;

        if (planetDatas.TryGetValue(guid, out planetData))
        {
            return planetData;
        }

        return null;
    }

    Vector3[] GetRandomSystemPoints()
    {
        Vector3[] systemPoints = new Vector3[systemCount];
        Vector3 systemPoint;
        float minDistance;
        float minDistanceSqrd = minSystemDistance * minSystemDistance;

        for(int i = 0; i < systemCount; i++)
        {
            do
            {
                minDistance = Mathf.Infinity;
                systemPoint = new Vector3(UnityEngine.Random.Range(-galaxySize, galaxySize), -1f, UnityEngine.Random.Range(-galaxySize, galaxySize));

                for (int j = i - 1; j >= 0; j--)
                {
                    float distance = (systemPoint - systemPoints[j]).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }

            } while (minDistance < minDistanceSqrd);

            systemPoints[i] = systemPoint;
        }

        return systemPoints;
    }

    Vector3[] GetRandomPlanetPositions(int count)
    {
        Vector3[] planetPositions = new Vector3[count];

        float radius = UnityEngine.Random.Range(0.5f, 1.0f);

        for(int i = 0; i < count; i++)
        {
            Vector2 direction = StaticHelpers.GetRandomDirection() * radius;
            planetPositions[i] = new Vector3(direction.x, 0f, direction.y);
            radius += UnityEngine.Random.Range(0.5f, 1.0f);
        }

        return planetPositions;
    }
}

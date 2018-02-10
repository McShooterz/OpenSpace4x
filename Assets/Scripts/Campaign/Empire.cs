/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: Players are instances of the Empire class
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Empire : MonoBehaviour
{
    [SerializeField]
    bool isPlayer = false;

    [SerializeField]
    List<ShipData> Ships = new List<ShipData>();

    [SerializeField]
    List<FleetData> Fleets = new List<FleetData>();

    [SerializeField]
    FleetManager fleetManager;

    [SerializeField]
    PlanetManager planetManager;





    [SerializeField]
    string displayName;




    [SerializeField]
    float influence;

    [SerializeField]
    float influenceMax;

    [Header("Technology")]

    [SerializeField]
    float researchPoolPhysics = 0f;

    [SerializeField]
    float researchPoolSociety = 0f;

    [SerializeField]
    float researchPoolEngineering = 0f;

    [SerializeField]
    TechnologyTree technologyTree;

    [SerializeField]
    TechnologyEntry activeResearchPhysics = null;

    [SerializeField]
    TechnologyEntry activeResearchSociety = null;

    [SerializeField]
    TechnologyEntry activeResearchEngineering = null;

    [SerializeField]
    List<TechnologyEntry> currentTechnologiesPhysics = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> currentTechnologiesSociety = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> currentTechnologiesEngineering = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> researchedTechnologiesPhysics = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> researchedTechnologiesSociety = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> researchedTechnologiesEngineering = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> physicsTechnologyEntries = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> societyTechnologyEntries = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> engineeringTechnologyEntries = new List<TechnologyEntry>();

    void Start()
    {
        technologyTree = ResourceManager.instance.GetTechnologyTree("Default");
        BuildTechnologyEntries(technologyTree);
    }

    void Update()
    {
        
    }

    public void AddShipData(string DesignName)
    {
        //ShipData shipData = new ShipData(ResourceManager.shipDesignDatas[DesignName]);
        //shipData.Owner = this;
        //Temp add galaxy class
        //Ships.Add(shipData);
    }

    public void AddEmptyFleet(Vector3 Position)
    {
        FleetData FD = new FleetData();
        FD.Owner = this;
        fleetManager.CreateFleet(FD, Position);
    }

    public FleetManager GetFleetManager()
    {
        return fleetManager;
    }

    public PlanetManager GetPlanetManager()
    {
        return planetManager;
    }

    public string GetDisplayName()
    {
        return displayName;
    }


    public float GetInfluence()
    {
        return influence;
    }

    public TechnologyEntry GetActiveResearchPhysics()
    {
        return activeResearchPhysics;
    }

    public void SetActiveResearchPhysics(TechnologyEntry techEntry)
    {
        activeResearchPhysics = techEntry;
    }

    public TechnologyEntry GetActiveResearchSociety()
    {
        return activeResearchSociety;
    }

    public void SetActiveResearchSociety(TechnologyEntry techEntry)
    {
        activeResearchSociety = techEntry;
    }

    public TechnologyEntry GetActiveResearchEngineering()
    {
        return activeResearchEngineering;
    }

    public void SetActiveResearchEngineering(TechnologyEntry techEntry)
    {
        activeResearchEngineering = techEntry;
    }

    public void SetDisplayName(string name)
    {
        displayName = name;
    }

    public void SetInfluence(float value)
    {
        influence = value;
    }

    public void AddInfluence(float addValue)
    {
        influence += addValue;

        if (influence >= influenceMax)
        {
            influence = influenceMax;
        }
    }

    public float GetInfluenceMax()
    {
        return influenceMax;
    }

    public void SetInfluenceMax(float value)
    {
        influenceMax = value;
    }

    public void BuildTechnologyEntries(TechnologyTree technologyTree)
    {
        TechnologyEntry techEntry;

        foreach (Technology tech in technologyTree.GetPhysicsTechnologies())
        {
            techEntry = ScriptableObject.CreateInstance("TechnologyEntry") as TechnologyEntry;
            techEntry.name = tech.GetName();
            techEntry.SetTechnology(tech);
            physicsTechnologyEntries.Add(techEntry);
        }

        foreach (Technology tech in technologyTree.GetSocietyTechnologies())
        {
            techEntry = ScriptableObject.CreateInstance("TechnologyEntry") as TechnologyEntry;
            techEntry.name = tech.GetName();
            techEntry.SetTechnology(tech);
            societyTechnologyEntries.Add(techEntry);
        }

        foreach (Technology tech in technologyTree.GetEngineeringTechnologies())
        {
            techEntry = ScriptableObject.CreateInstance("TechnologyEntry") as TechnologyEntry;
            techEntry.name = tech.GetName();
            techEntry.SetTechnology(tech);
            engineeringTechnologyEntries.Add(techEntry);
        }

        GenerateCurrentTechnologiesPhysics(3);
        GenerateCurrentTechnologiesSociety(3);
        GenerateCurrentTechnologiesEngineering(3);
    }

    public bool HasResearchedTechnology(Technology technology)
    {
        foreach (TechnologyEntry techEntry in physicsTechnologyEntries)
        {
            if (technology == techEntry.GetTechnology())
            {
                return techEntry.IsCompleted();
            }
        }

        foreach (TechnologyEntry techEntry in societyTechnologyEntries)
        {
            if (technology == techEntry.GetTechnology())
            {
                return techEntry.IsCompleted();
            }
        }

        foreach (TechnologyEntry techEntry in engineeringTechnologyEntries)
        {
            if (technology == techEntry.GetTechnology())
            {
                return techEntry.IsCompleted();
            }
        }

        return false;
    }

    public List<TechnologyEntry> GetCurrentTechnologiesPhysics()
    {
        return currentTechnologiesPhysics;
    }

    public List<TechnologyEntry> GetCurrentTechnologiesSociety()
    {
        return currentTechnologiesSociety;
    }

    public List<TechnologyEntry> GetCurrentTechnologiesEngineering()
    {
        return currentTechnologiesEngineering;
    }

    public void GenerateCurrentTechnologiesPhysics(int count)
    {
        List<TechnologyEntry> availableTechEntries = new List<TechnologyEntry>();

        foreach (TechnologyEntry techEntry in physicsTechnologyEntries)
        {
            if (!techEntry.IsCompleted() && techEntry.GetTechnology().MeetsRequirements(this))
            {
                availableTechEntries.Add(techEntry);
            }
        }

        if (availableTechEntries.Count < count)
        {
            count = availableTechEntries.Count;
        }

        if (availableTechEntries.Count == count)
        {
            currentTechnologiesPhysics = availableTechEntries;
        }
        else
        {
            HashSet<int> randomNumbers = new HashSet<int>();
            int[] randomIndices = new int[count];

            while (randomNumbers.Count < count)
            {
                randomNumbers.Add(Random.Range(0, availableTechEntries.Count));
            }

            randomNumbers.CopyTo(randomIndices);

            for (int i = 0; i < count; i++)
            {
                currentTechnologiesPhysics.Add(availableTechEntries[randomIndices[i]]);
            }
        }
    }

    public void GenerateCurrentTechnologiesSociety(int count)
    {
        List<TechnologyEntry> availableTechEntries = new List<TechnologyEntry>();

        foreach (TechnologyEntry techEntry in societyTechnologyEntries)
        {
            if (!techEntry.IsCompleted() && techEntry.GetTechnology().MeetsRequirements(this))
            {
                availableTechEntries.Add(techEntry);
            }
        }

        if (availableTechEntries.Count < count)
        {
            count = availableTechEntries.Count;
        }

        if (availableTechEntries.Count == count)
        {
            currentTechnologiesSociety = availableTechEntries;
        }
        else
        {
            HashSet<int> randomNumbers = new HashSet<int>();
            int[] randomIndices = new int[count];

            while (randomNumbers.Count < count)
            {
                randomNumbers.Add(Random.Range(0, availableTechEntries.Count));
            }

            randomNumbers.CopyTo(randomIndices);

            for (int i = 0; i < count; i++)
            {
                currentTechnologiesSociety.Add(availableTechEntries[randomIndices[i]]);
            }
        }
    }

    public void GenerateCurrentTechnologiesEngineering(int count)
    {
        List<TechnologyEntry> availableTechEntries = new List<TechnologyEntry>();

        foreach (TechnologyEntry techEntry in engineeringTechnologyEntries)
        {
            if (!techEntry.IsCompleted() && techEntry.GetTechnology().MeetsRequirements(this))
            {
                availableTechEntries.Add(techEntry);
            }
        }

        if (availableTechEntries.Count < count)
        {
            count = availableTechEntries.Count;
        }

        if (availableTechEntries.Count == count)
        {
            currentTechnologiesEngineering = availableTechEntries;
        }
        else
        {
            HashSet<int> randomNumbers = new HashSet<int>();
            int[] randomIndices = new int[count];

            while (randomNumbers.Count < count)
            {
                randomNumbers.Add(Random.Range(0, availableTechEntries.Count));
            }

            randomNumbers.CopyTo(randomIndices);

            for (int i = 0; i < count; i++)
            {
                currentTechnologiesEngineering.Add(availableTechEntries[randomIndices[i]]);
            }
        }
    }

    public void ChangeDay()
    {

    }

    public void ChangeMonth()
    {
        researchPoolPhysics += 100f;
        researchPoolSociety += 100f;
        researchPoolEngineering += 100f;


        if (activeResearchPhysics != null)
        {
            researchPoolPhysics = activeResearchPhysics.ApplyResearchPoints(researchPoolPhysics, 1.0f);

            if (activeResearchPhysics.IsCompleted())
            {
                researchedTechnologiesPhysics.Add(activeResearchPhysics);
                activeResearchPhysics = null;
                GenerateCurrentTechnologiesPhysics(3);
            }
        }

        if (activeResearchSociety != null)
        {
            researchPoolSociety = activeResearchSociety.ApplyResearchPoints(researchPoolSociety, 1.0f);

            if (activeResearchSociety.IsCompleted())
            {
                researchedTechnologiesSociety.Add(activeResearchSociety);
                activeResearchSociety = null;
                GenerateCurrentTechnologiesSociety(3);
            }
        }

        if (activeResearchEngineering != null)
        {
            researchPoolEngineering = activeResearchEngineering.ApplyResearchPoints(researchPoolEngineering, 1.0f);

            if (activeResearchEngineering.IsCompleted())
            {
                researchedTechnologiesEngineering.Add(activeResearchEngineering);
                activeResearchEngineering = null;
                GenerateCurrentTechnologiesEngineering(3);
            }
        }
    }
}

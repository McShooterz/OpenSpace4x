using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmpireData : Object
{
# region Variables

    [SerializeField]
    string displayName;




    [SerializeField]
    float influence;

    [SerializeField]
    float influenceMax;


    [SerializeField]
    TechnologyEntry activeResearchPhysics = null;

    [SerializeField]
    TechnologyEntry activeResearchSociety = null;

    [SerializeField]
    TechnologyEntry activeResearchEngineering = null;

    [SerializeField]
    TechnologyEntry[] currentTechnologiesPhysics;

    [SerializeField]
    TechnologyEntry[] currentTechnologiesSociety;

    [SerializeField]
    TechnologyEntry[] currentTechnologiesEngineering;

    [SerializeField]
    List<TechnologyEntry> physicsTechnologyEntries = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> societyTechnologyEntries = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> engineeringTechnologyEntries = new List<TechnologyEntry>();

# endregion

    public EmpireData()
    {


        BuildTechnologyEntries(ResourceManager.instance.GetTechnologyTree("Default"));

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

    public TechnologyEntry GetActiveResearchSociety()
    {
        return activeResearchSociety;
    }

    public TechnologyEntry GetActiveResearchEngineering()
    {
        return activeResearchEngineering;
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
        foreach(Technology tech in technologyTree.GetPhysicsTechnologies())
        {
            physicsTechnologyEntries.Add(new TechnologyEntry(tech));
        }

        foreach (Technology tech in technologyTree.GetSocietyTechnologies())
        {
            societyTechnologyEntries.Add(new TechnologyEntry(tech));
        }

        foreach (Technology tech in technologyTree.GetEngineeringTechnologies())
        {
            engineeringTechnologyEntries.Add(new TechnologyEntry(tech));
        }
    }

    public bool HasResearchedTechnology(Technology technology)
    {
        foreach(TechnologyEntry techEntry in physicsTechnologyEntries)
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

    public TechnologyEntry[] GetCurrentTechnologiesPhysics()
    {
        if (currentTechnologiesPhysics.Length == 0)
        {
            GenerateCurrentTechnologiesPhysics(3);
        }

        return currentTechnologiesPhysics;
    }

    public TechnologyEntry[] GetCurrentTechnologiesSociety()
    {
        if (currentTechnologiesSociety.Length == 0)
        {
            GenerateCurrentTechnologiesSociety(3);
        }

        return currentTechnologiesSociety;
    }

    public TechnologyEntry[] GetCurrentTechnologiesEngineering()
    {
        if (currentTechnologiesEngineering.Length == 0)
        {
            GenerateCurrentTechnologiesEngineering(3);
        }

        return currentTechnologiesEngineering;
    }

    public void GenerateCurrentTechnologiesPhysics(int count)
    {
        GenerateCurrentTechnologies(count, currentTechnologiesPhysics, physicsTechnologyEntries);
    }

    public void GenerateCurrentTechnologiesSociety(int count)
    {
        GenerateCurrentTechnologies(count, currentTechnologiesSociety, societyTechnologyEntries);
    }

    public void GenerateCurrentTechnologiesEngineering(int count)
    {
        GenerateCurrentTechnologies(count, currentTechnologiesEngineering, engineeringTechnologyEntries);
    }

    void GenerateCurrentTechnologies(int count, TechnologyEntry[] currentArray, List<TechnologyEntry> sourceList)
    {
        List<TechnologyEntry> availableTechEntries = new List<TechnologyEntry>();

        foreach (TechnologyEntry techEntry in sourceList)
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

        currentArray = new TechnologyEntry[count];

        if (availableTechEntries.Count == count)
        {
            for (int i = 0; i < count; i++)
            {
                currentArray[i] = availableTechEntries[i];
            }
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
                currentArray[i] = availableTechEntries[randomIndices[i]];
            }
        }
    }

    public void ChangeDay()
    {

    }

    public void ChangeMonth()
    {
        if (activeResearchPhysics != null)
        {

        }

        if (activeResearchSociety != null)
        {

        }

        if (activeResearchEngineering != null)
        {

        }
    }
}

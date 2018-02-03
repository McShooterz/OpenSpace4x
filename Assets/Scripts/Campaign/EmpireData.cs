using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmpireData
{

    [SerializeField]
    string displayName;




    [SerializeField]
    float influence;

    [SerializeField]
    float influenceMax;


    [SerializeField]
    TechnologyEntry activePhysicsResearch;

    [SerializeField]
    TechnologyEntry activeSocietyResearch;

    [SerializeField]
    TechnologyEntry activeEngineeringResearch;

    [SerializeField]
    List<TechnologyEntry> physicsTechnologyEntries = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> societyTechnologyEntries = new List<TechnologyEntry>();

    [SerializeField]
    List<TechnologyEntry> engineeringTechnologyEntries = new List<TechnologyEntry>();

    public string GetDisplayName()
    {
        return displayName;
    }

    public void SetDisplayName(string name)
    {
        displayName = name;
    }

    public float GetInfluence()
    {
        return influence;
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

    public void ChangeDay()
    {

    }

    public void ChangeMonth()
    {
        if (activePhysicsResearch != null)
        {

        }

        if (activeSocietyResearch != null)
        {

        }

        if (activeEngineeringResearch != null)
        {

        }
    }
}

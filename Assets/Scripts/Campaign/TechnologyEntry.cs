using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TechnologyEntry
{

    [SerializeField]
    Technology technology;


    [SerializeField]
    float researchPoints;

    [SerializeField]
    bool completed;


    public TechnologyEntry(Technology tech)
    {
        technology = tech;
        researchPoints = 0;
        completed = false;
    }

    public Technology GetTechnology()
    {
        return technology;
    }

    public float GetResearchPoints()
    {
        return researchPoints;
    }

    public bool IsCompleted()
    {
        return completed;
    }

    public void SetCompleted()
    {
        completed = true;
    }
}

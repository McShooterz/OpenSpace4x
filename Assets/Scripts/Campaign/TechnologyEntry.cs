using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TechnologyEntry : ScriptableObject
{
    [SerializeField]
    Technology technology;

    [SerializeField]
    float researchPoints;

    [SerializeField]
    bool completed;

    public void SetTechnology(Technology tech)
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

    public float GetResearchCost(float modifier)
    {
        return technology.baseCost * modifier;
    }

    public float GetResearchPercentCompleted(float modifier)
    {
        if (researchPoints == 0f)
        {
            return 0f;
        }

        float percent = researchPoints / technology.baseCost * modifier;

        return Mathf.Clamp(percent, 0f, 1.0f);
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

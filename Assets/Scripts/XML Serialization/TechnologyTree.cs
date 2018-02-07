using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TechnologyTree
{
    string[] physcisTechnologies;
    string[] societyTechnologies;
    string[] engineeringTechnologies;

    public List<Technology> GetPhysicsTechnologies()
    {
        List<Technology> techList = new List<Technology>();

        foreach(string techName in physcisTechnologies)
        {
            Technology tech = ResourceManager.instance.GetTechnology(techName);

            if (tech != null)
            {
                techList.Add(tech);
            }
        }

        return techList;
    }

    public List<Technology> GetSocietyTechnologies()
    {
        List<Technology> techList = new List<Technology>();

        foreach (string techName in societyTechnologies)
        {
            Technology tech = ResourceManager.instance.GetTechnology(techName);

            if (tech != null)
            {
                techList.Add(tech);
            }
        }

        return techList;
    }

    public List<Technology> GetEngineeringTechnologies()
    {
        List<Technology> techList = new List<Technology>();

        foreach (string techName in engineeringTechnologies)
        {
            Technology tech = ResourceManager.instance.GetTechnology(techName);

            if (tech != null)
            {
                techList.Add(tech);
            }
        }

        return techList;
    }
}

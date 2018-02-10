using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TechnologyTree
{
    public string[] startingPhyscisTechnologies = new string[0];
    public string[] startingSocietyTechnologies = new string[0];
    public string[] startingEngineeringTechnologies = new string[0];
    public string[] physcisTechnologies = new string[0];
    public string[] societyTechnologies = new string[0];
    public string[] engineeringTechnologies = new string[0];

    public List<Technology> GetStartingPhysicsTechnologies()
    {
        return GetTechnologyList(startingPhyscisTechnologies);
    }

    public List<Technology> GetStartingSocietyTechnologies()
    {
        return GetTechnologyList(startingSocietyTechnologies);
    }

    public List<Technology> GetStartingEngineeringTechnologies()
    {
        return GetTechnologyList(startingEngineeringTechnologies);
    }

    public List<Technology> GetPhysicsTechnologies()
    {
        return GetTechnologyList(physcisTechnologies);
    }

    public List<Technology> GetSocietyTechnologies()
    {
        return GetTechnologyList(societyTechnologies);
    }

    public List<Technology> GetEngineeringTechnologies()
    {
        return GetTechnologyList(engineeringTechnologies);
    }

    List<Technology> GetTechnologyList(string[] technologyNames)
    {
        List<Technology> technologyList = new List<Technology>();

        foreach (string technologyName in technologyNames)
        {
            Technology technology = ResourceManager.instance.GetTechnology(technologyName);

            if (technology != null)
            {
                technologyList.Add(technology);
            }
        }

        return technologyList;
    }
}

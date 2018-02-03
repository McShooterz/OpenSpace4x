using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TechnologyTree
{
    Technology[] physcisTechnologies;
    Technology[] societyTechnologies;
    Technology[] engineeringTechnologies;

    public Technology[] GetPhysicsTechnologies()
    {
        return physcisTechnologies;
    }

    public Technology[] GetSocietyTechnologies()
    {
        return societyTechnologies;
    }

    public Technology[] GetEngineeringTechnologies()
    {
        return engineeringTechnologies;
    }
}

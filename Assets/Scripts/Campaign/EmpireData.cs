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
}

/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    [SerializeField]
    string name;

    [SerializeField]
    float age;

    [SerializeField]
    int level;

    [SerializeField]
    float experience;

    [SerializeField]
    List<CharacterTrait> traits = new List<CharacterTrait>();

    [SerializeField]
    List<CharacterSkill> skills = new List<CharacterSkill>();

    public string GetName()
    {
        return name;
    }

    public void SetName(string newName)
    {
        name = newName;
    }

    public float GetAge()
    {
        return age;
    }

    public void SetAge(float value)
    {
        age = value;
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetLevel(int value)
    {
        level = value;
    }

    public void AddLevel(int value)
    {
        level += value;
    }

    public float GetExperience()
    {
        return experience;
    }

    public void SetExperience(float value)
    {
        experience = value;
    }

    public void AddExperience(float value)
    {
        experience += value;
    }

    public List<CharacterTrait> GetTraits()
    {
        return traits;
    }

    public List<CharacterSkill> GetSkills()
    {
        return skills;
    }
}

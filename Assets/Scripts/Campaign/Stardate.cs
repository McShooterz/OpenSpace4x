using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stardate
{
    [SerializeField]
    int year;

    [SerializeField]
    int month;

    [SerializeField]
    int day;

    int[] daysPerMonth = new int[12]
    {
        31,28,31,30,31,30,31,31,30,31,30,31
    };

    public void SetStardate(int setYear, int setMonth, int setDay)
    {
        year = setYear;
        month = setMonth;
        day = setDay;
    }

    public int GetYear()
    {
        return year;
    }

    public int GetMonth()
    {
        return month;
    }

    public int GetDay()
    {
        return day;
    }
}

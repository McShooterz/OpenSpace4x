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
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class WeaponDamageGraph : MonoBehaviour
{
    [SerializeField]
    UILineRenderer Lines;

    [SerializeField]
    Text DamageLabel;

    [SerializeField]
    Text RangeLabel;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ResetGraph()
    {
        Lines.Points = new Vector2[0];
    }

    public void SetWeapon(Weapon weapon)
    {
        DamageLabel.text = ResourceManager.GetLocalization("Damage") + ": " + weapon.GetMaxDamage().ToString("0");
        RangeLabel.text = ResourceManager.GetLocalization("Range") + ": " + weapon.GetMaxRangeDisplay().ToString("0");

        Rect GraphRect = Lines.GetComponent<RectTransform>().rect;

        Vector2[] DamagePoints = weapon.GetGraphDamagePoints();
        Lines.Points = new Vector2[DamagePoints.Length];

        for (int i = 0; i < DamagePoints.Length; i++)
        {
            Lines.Points[i] = new Vector2(DamagePoints[i].x * GraphRect.width, DamagePoints[i].y * GraphRect.height);
        }
    }
}

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

public class StatEntryList : MonoBehaviour
{
    [SerializeField]
    GameObject StatEntryPrefab;

    List<StatEntry> StatEntries = new List<StatEntry>();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Clear()
    {
        foreach(StatEntry stat in StatEntries)
        {
            Destroy(stat.gameObject);
        }
        StatEntries.Clear();
    }

    public void AddEntry(Texture2D texture, string text)
    {
        GameObject StatEntryObject = Instantiate(StatEntryPrefab);
        StatEntryObject.transform.SetParent(transform);
        StatEntryObject.transform.localScale = Vector3.one;
        StatEntry statEntry = StatEntryObject.GetComponent<StatEntry>();
        statEntry.SetImage(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
        statEntry.SetText(text);
        StatEntries.Add(statEntry);
    }
}

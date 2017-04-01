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

public class BaseScrollList : MonoBehaviour
{
    [SerializeField]
    protected GameObject EntryPrefab;

    [SerializeField]
    protected GameObject ScrollListContent;

    List<GameObject> EntryList = new List<GameObject>();

    // Use this for initialization
    protected virtual void Start ()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
		
	}

    public GameObject InstantiateEntry(string Name)
    {
        GameObject newEntry = Instantiate(EntryPrefab);
        newEntry.name = Name;
        RectTransform entryRectTransform = newEntry.GetComponent<RectTransform>();
        entryRectTransform.SetParent(ScrollListContent.GetComponent<RectTransform>());
        entryRectTransform.localScale = Vector3.one;
        EntryList.Add(newEntry);
        return newEntry;
    }

    public void Clear()
    {
        foreach(GameObject entry in EntryList)
        {
            Destroy(entry);
        }
        EntryList.Clear();
    }
}

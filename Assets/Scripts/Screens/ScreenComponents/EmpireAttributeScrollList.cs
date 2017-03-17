/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class EmpireAttributeScrollList : ScrollListBase
{
    List<EmpireAttributeEntry> EmpireAttributes = new List<EmpireAttributeEntry>();

    EmpireAttributeEntry.ButtonPress ButtonPress;

	public EmpireAttributeScrollList(Rect rect, EmpireAttributeType attributeType, EmpireAttributeEntry.ButtonPress buttonPress)
    {
        ScrollWindowRect = rect;

        ButtonPress = buttonPress;

        ScrollViewRect = new Rect(0,0, ScrollWindowRect.width * 0.88f, ScrollWindowRect.height * 5f);

        ListEntrySize = new Vector2(ScrollViewRect.width, ScrollWindowRect.height / 4f);

        foreach(KeyValuePair<string, EmpireAttribute> keyVal in ResourceManager.EmpireAttributes)
        {
            if(keyVal.Value.AttributeType == attributeType)
            {
                AddEntry(keyVal);
            }
        }
        ScaleView();
    }

    public void Draw()
    {
        ScrollPosition = GUI.BeginScrollView(ScrollWindowRect, ScrollPosition, ScrollViewRect);
        foreach (EmpireAttributeEntry entry in EmpireAttributes)
        {
            entry.Draw(SelectionIndex);
        }
        GUI.EndScrollView();
    }

    public void AddEntry(KeyValuePair<string, EmpireAttribute> empireAttribute)
    {
        Rect EntryRect = new Rect(0, ListEntrySize.y * EmpireAttributes.Count, ListEntrySize.x, ListEntrySize.y);
        EmpireAttributes.Add(new EmpireAttributeEntry(EntryRect, empireAttribute, EmpireAttributes.Count, ButtonPress, ChangeSelectionIndex));
    }

    public EmpireAttribute GetFirstEntry()
    {
        if(EmpireAttributes.Count > 0)
        {
            return EmpireAttributes[0].Attribute.Value;
        }
        return null;
    }

    public EmpireAttribute GetSelectedEntry()
    {
        if (EmpireAttributes.Count > 0)
        {
            return EmpireAttributes[SelectionIndex].Attribute.Value;
        }
        return null;
    }

    public string GetSelectedEntryName()
    {
        if (EmpireAttributes.Count > 0)
        {
            return EmpireAttributes[SelectionIndex].Attribute.Key;
        }
        return "";
    }

    public void SetSelectedEntry(string AttributeName)
    {
        for(int i = 0; i < EmpireAttributes.Count; i++)
        {
            if(EmpireAttributes[i].Attribute.Key == AttributeName)
            {
                SelectionIndex = i;
                EmpireAttributes[i].Select();
            }
        }
    }

    public void ScaleView()
    {
        ScrollViewRect.height = Mathf.Max(EmpireAttributes.Count * ListEntrySize.y, ScrollWindowRect.height * 1.05f);
    }

    public void Clear()
    {
        EmpireAttributes.Clear();
    }
}

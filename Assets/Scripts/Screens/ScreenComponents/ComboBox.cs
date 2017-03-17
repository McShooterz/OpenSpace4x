/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ComboBox
{
    Rect baseRect;

    string SelectedEntryName = "";

    Rect ScrollWindowRect;
    Rect ScrollViewRect;
    Vector2 ScrollPosition;

    List<ComboBoxEntry> Entries = new List<ComboBoxEntry>();

    bool Open;

    public ComboBox(Rect rect)
    {
        baseRect = rect;

        ScrollWindowRect = new Rect(baseRect.x, baseRect.yMax, baseRect.width, baseRect.height * 5f);
        ScrollViewRect = new Rect(0,0, ScrollWindowRect.width * 0.9f, 0);
        ScrollPosition = Vector2.zero;
    }

    public void Draw(Vector2 mousePosition)
    {
        if(Open)
        {
            if (!baseRect.Contains(mousePosition) && !ScrollWindowRect.Contains(mousePosition))
                Open = false;

            GUI.Box(ScrollWindowRect, "", GameManager.instance.standardBackGround);

            ScrollPosition = GUI.BeginScrollView(ScrollWindowRect, ScrollPosition, ScrollViewRect);
            foreach (ComboBoxEntry entry in Entries)
            {
                entry.Draw(SelectedEntryName);
            }
            GUI.EndScrollView();
        }

        if (GUI.Button(baseRect, SelectedEntryName))
        {
            Open = true;
        }
    }

    public bool isOpen()
    {
        return Open;
    }

    public void Clear()
    {
        Entries.Clear();
        SelectedEntryName = "";
    }
    
    public void AddEntry(string entryName)
    {
        if (SelectedEntryName == "")
            SelectedEntryName = entryName;

        Rect rect = new Rect(0, GameManager.instance.StandardButtonSize.y * Entries.Count, ScrollViewRect.width, GameManager.instance.StandardButtonSize.y);
        Entries.Add(new ComboBoxEntry(rect, entryName, SelectEntry));

        ScrollViewRect.height = GameManager.instance.StandardButtonSize.y * Entries.Count;
    }

    public void SelectEntry(string name)
    {
        SelectedEntryName = name;
        Open = false;
    }

    public string GetSelectedEntry()
    {
        return SelectedEntryName;
    }

    public class ComboBoxEntry
    {
        Rect baseRect;
        string Name;

        public delegate void SelectEntry(string name);
        SelectEntry selectEntry;

        public ComboBoxEntry(Rect rect, string name, SelectEntry selectMethod)
        {
            baseRect = rect;
            Name = name;
            selectEntry = selectMethod;
        }

        public void Draw(string selectedName)
        {
            if (selectedName == Name)
            {
                if (GUI.Button(baseRect, Name, GameManager.instance.SquareButtonGreenStyle))
                {
                    selectEntry(Name);
                }
            }
            else
            {
                if (GUI.Button(baseRect, Name))
                {
                    selectEntry(Name);
                }
            }
        }
    }
}

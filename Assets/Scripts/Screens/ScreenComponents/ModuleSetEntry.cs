/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class ModuleSetEntry
{
    Rect Entry;
    Rect Icon;
    Rect Name;
    Rect Description;
    Rect Mod;
    public ModuleSet moduleSet;
    Texture2D Texture;

    string DisplayName;
    string DisplayDiscription;
    string DisplayMod = "";

    public delegate void ButtonPress(ModuleSet modSet);
    protected ButtonPress buttonCallBack;

    public ModuleSetEntry(Rect rect, ModuleSet mod, Texture2D tex, ButtonPress callBack)
    {
        Entry = rect;
        moduleSet = mod;
        Texture = tex;
        buttonCallBack = callBack;

        Icon = new Rect(Entry.x + Entry.height * 0.06f, Entry.y + Entry.height * 0.06f, Entry.height * 0.88f, Entry.height * 0.88f);
        Name = new Rect(Icon.xMax + Entry.height * 0.06f, Entry.y + Entry.height * 0.03f, Entry.width - Icon.width - Entry.height * 0.2f, Entry.height * 0.25f);
        Description = new Rect(Name.x, Name.yMax, Name.width, Entry.height * 0.5f);
        Mod = new Rect(Name.x, Description.yMax, Name.width, Entry.height * 0.25f);

        DisplayName = ResourceManager.instance.GetLocalization(moduleSet.Name);
        DisplayDiscription = ResourceManager.instance.GetLocalization(moduleSet.Description);

        if (moduleSet.GetParentMod() != null)
        {
            DisplayMod = "Mod: " + moduleSet.GetParentMod().Name;
        }
        else
        {
            DisplayMod = "Mod: N/A";
        }
    }

    //Pass in selectedModule to check if this entry is selected
    public void Draw(ModuleSet selected)
    {
        if (selected == moduleSet)
        {
            if (GUI.Button(Entry, "", GameManager.instance.SquareButtonGreenStyle))
            {
                buttonCallBack(moduleSet);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        else
        {
            if (GUI.Button(Entry, ""))
            {
                buttonCallBack(moduleSet);
                AudioManager.instance.PlayUIClip("SelectButtonClick");
            }
        }
        GUI.DrawTexture(Icon, Texture, ScaleMode.ScaleToFit);
        GUI.Label(Name, DisplayName, GameManager.instance.ModuleTitleStyle);
        GUI.Label(Description, DisplayDiscription, GameManager.instance.ModuleDescStyle);
        GUI.Label(Mod, DisplayMod, GameManager.instance.ModuleModStyle);
    }

    public ModuleSet GetModuleSet()
    {
        return moduleSet;
    }

    public Vector2 GetPosition()
    {
        return Entry.position;
    }
}

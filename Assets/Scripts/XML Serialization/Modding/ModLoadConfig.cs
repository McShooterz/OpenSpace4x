using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class ModLoadConfig
{
    public List<ModEntry> ModLoadOrder = new List<ModEntry>();

    public struct ModEntry
    {
        public ModEntry(string folderName, bool toLoad)
        {
            Name = folderName;
            Load = toLoad;
        }

        public string Name;
        public bool Load;
    }

    public bool hasMod(string modName)
    {
        foreach (ModEntry entry in ModLoadOrder)
        {
            if (entry.Name == modName)
                return true;
        }
        return false;
    }
}

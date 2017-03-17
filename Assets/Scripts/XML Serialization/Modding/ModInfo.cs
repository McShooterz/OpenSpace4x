using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class ModInfo
{
    public string Name;
    public string Version;
    public string Description;
    string path;

    public void SetPath(string newPath)
    {
        path = newPath;
    }

    public string GetPath()
    {
        return path;
    }
}

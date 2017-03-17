using UnityEngine;
using System.Collections.Generic;

public class Scenario
{
    public string Name;
    public string Description;
    public float TimeLimit;
    public int CommandLimit;
    public float MoneyLimit;
    public ScenarioDifficulty Difficulty;
    public Vector3 DeploymentAreaPosition;

    public List<ShipEntry> PlayerShips = new List<ShipEntry>();
    public List<ShipEntry> EnemyShips = new List<ShipEntry>();
    public List<ShipEntry> AlliedShips = new List<ShipEntry>();
    public List<ShipEntry> NeutralShips = new List<ShipEntry>();

    public bool Deleted = false;
    //Path is set by code, no need to set in file.
    string path = "";

    public Scenario(){}

    public void SetPath(string newPath)
    {
        path = newPath;
    }

    public string GetPath()
    {
        return path;
    }

    public class ShipEntry
    {
        public string Name;
        public string Hull;
        public string Design;
        public int Level;
        public Vector3 Position;
        public Quaternion Rotation;

        public ShipEntry(){}

        public ShipEntry(string name, string hull, string design, int level, GameObject shipObject)
        {
            Name = name;
            Hull = hull;
            Design = design;
            Level = level;
            Position = shipObject.transform.position;
            Rotation = shipObject.transform.rotation;
        }
    }
}

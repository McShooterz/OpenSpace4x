using UnityEngine;
using System.Collections.Generic;

public class Scenario
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float TimeLimit { get; set; }
    public int CommandLimit { get; set; }
    public float MoneyLimit { get; set; }
    public ScenarioDifficulty Difficulty { get; set; }
    public Vector3 DeploymentAreaPosition { get; set; }

    public List<ShipEntry> PlayerShips { get; set; }
    public List<ShipEntry> EnemyShips { get; set; }
    public List<ShipEntry> AlliedShips { get; set; }
    public List<ShipEntry> NeutralShips { get; set; }

    public bool Deleted { get; set; }

    //Path is set by code, no need to set in file.
    string path = "";

    public Scenario()
    {
        PlayerShips = new List<ShipEntry>();
        EnemyShips = new List<ShipEntry>();
        AlliedShips = new List<ShipEntry>();
        NeutralShips = new List<ShipEntry>();
    }

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

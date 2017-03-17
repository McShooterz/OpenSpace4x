/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

public class HardPointSaver : MonoBehaviour
{
    public GameObject[] HardPointsSources;

	// Use this for initialization
	void Start ()
    {
	    if(HardPointsSources.Length > 0)
        {
            Directory.CreateDirectory(Application.dataPath + "/ShipHardPoints");
            foreach (GameObject source in HardPointsSources)
            {
                HardPoints hardPoints = source.GetComponent<HardPoints>();
                if (hardPoints != null)
                {
                    HardPointsStored hardPointsStored = new HardPointsStored(hardPoints);

                    string path = Application.dataPath + "/ShipHardPoints/" + source.name + ".xml";
                    XmlSerializer Writer = new XmlSerializer(typeof(HardPointsStored));
                    FileStream file = File.Create(path);
                    Writer.Serialize(file, hardPointsStored);
                    file.Close();
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

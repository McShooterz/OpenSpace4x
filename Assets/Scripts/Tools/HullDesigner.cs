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

public class HullDesigner : MonoBehaviour
{
    //Textures for different slot quadrant assignments
    public Texture2D Tex_Fore, Tex_Aft, Tex_Port, Tex_Starboard, Tex_Center;

    [Tooltip("Drag and drop an existing slot layout to load on play.")]
    public TextAsset loadLayout;

    int CountFore, CountAft, CountPort, CountStarboard, CountCenter;
    float Indent;

    List<Slot> Slots = new List<Slot>();

    QuadrantTypes selectedQuadrantType;

    //Buttons
    Rect QuadrantTypeButtonRect;
    Rect ResestButtonRect;
    Rect SaveButtonRect;

    //Info
    Rect ForeCountRect;
    Rect AftCountRect;
    Rect PortCountRect;
    Rect StarboardCountRect;
    Rect CenterCountRect;
    Rect TotalCountRect;

    Rect NameFieldRect;
    string LayoutName = "";

	// Use this for initialization
	void Start ()
    {
        //Force Camera to correct position
        Camera.main.transform.position = new Vector3(0f,5f,0f);
        Camera.main.transform.rotation = Quaternion.Euler(90,0,0);

        Indent = Screen.width * 0.15f;
        //Build all of the rectangles for slots
        Slots.Clear();
        for(int x = (int)Indent; x < Screen.width - 16; x += 16)
        {
            for(int y = 0; y < Screen.height - 16; y += 16)
            {
                Slots.Add(new Slot(new Rect(x,y,16,16)));
            }
        } 

        if(loadLayout != null)
        {
            LayoutName = loadLayout.name;
            ShipSlotLayout loadedLayout = (ShipSlotLayout)new XmlSerializer(typeof(ShipSlotLayout)).Deserialize(new StringReader(loadLayout.text));

            foreach (Rect foreslot in loadedLayout.ForeSlots)
            {
                foreach(Slot slot in Slots)
                {
                    if(foreslot == slot.rect)
                    {
                        slot.Quadrant = QuadrantTypes.Fore;
                        slot.Active = true;
                        ChangeCount(slot.Quadrant, 1);
                    }
                }
            }
            foreach (Rect foreslot in loadedLayout.AftSlots)
            {
                foreach (Slot slot in Slots)
                {
                    if (foreslot == slot.rect)
                    {
                        slot.Quadrant = QuadrantTypes.Aft;
                        slot.Active = true;
                        ChangeCount(slot.Quadrant, 1);
                    }
                }
            }
            foreach (Rect foreslot in loadedLayout.PortSlots)
            {
                foreach (Slot slot in Slots)
                {
                    if (foreslot == slot.rect)
                    {
                        slot.Quadrant = QuadrantTypes.Port;
                        slot.Active = true;
                        ChangeCount(slot.Quadrant, 1);
                    }
                }
            }
            foreach (Rect foreslot in loadedLayout.StarboardSlots)
            {
                foreach (Slot slot in Slots)
                {
                    if (foreslot == slot.rect)
                    {
                        slot.Quadrant = QuadrantTypes.Starboard;
                        slot.Active = true;
                        ChangeCount(slot.Quadrant, 1);
                    }
                }
            }
            foreach (Rect foreslot in loadedLayout.CenterSlots)
            {
                foreach (Slot slot in Slots)
                {
                    if (foreslot == slot.rect)
                    {
                        slot.Quadrant = QuadrantTypes.Center;
                        slot.Active = true;
                        ChangeCount(slot.Quadrant, 1);
                    }
                }
            }
        }

        selectedQuadrantType = QuadrantTypes.Fore;
        QuadrantTypeButtonRect = new Rect(Screen.width * 0.05f, 0f, 100f,40f);
        ResestButtonRect = new Rect(QuadrantTypeButtonRect.x, QuadrantTypeButtonRect.y + 60f,100f,40f);
        SaveButtonRect = new Rect(ResestButtonRect.x, ResestButtonRect.y + 60f,100f,40f);

        NameFieldRect = new Rect(Screen.width * 0.01f, SaveButtonRect.y + 60f, Screen.width * 0.13f, 30f);

        ForeCountRect = new Rect(ResestButtonRect.x, NameFieldRect.y + 60f, 100f, 40f);
        AftCountRect = new Rect(ResestButtonRect.x, ForeCountRect.y + 60f, 100f, 40f);
        PortCountRect = new Rect(ResestButtonRect.x, AftCountRect.y + 60f, 100f, 40f);
        StarboardCountRect = new Rect(ResestButtonRect.x, PortCountRect.y + 60f, 100f, 40f);
        CenterCountRect = new Rect(ResestButtonRect.x, StarboardCountRect.y + 60f, 100f, 40f);
        TotalCountRect = new Rect(ResestButtonRect.x, CenterCountRect.y + 60f, 100f, 40f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Get mouse interactions
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            foreach(Slot slot in Slots)
            {
                if(slot.rect.Contains(mousePosition))
                {
                    if(slot.Active && slot.Quadrant != selectedQuadrantType)
                    {
                        ChangeCount(slot.Quadrant, -1);
                        ChangeCount(selectedQuadrantType, 1);
                    }
                    else if(!slot.Active)
                    {
                        slot.Active = true;
                        ChangeCount(selectedQuadrantType, 1);
                    }
                    slot.Quadrant = selectedQuadrantType;
                }
            }
        }
        if(Input.GetMouseButton(1))
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            foreach (Slot slot in Slots)
            {
                if (slot.rect.Contains(mousePosition) && slot.Active)
                {
                    slot.Active = false;
                    ChangeCount(slot.Quadrant, -1);
                }
            }
        }
	}


    void OnGUI()
    {
        //Draw slots to screen
        foreach (Slot slot in Slots)
        {
            if (slot.Active)
            {
                switch (slot.Quadrant)
                {
                    case QuadrantTypes.Fore:
                        {
                            GUI.DrawTexture(slot.rect, Tex_Fore);
                            break;
                        }
                    case QuadrantTypes.Aft:
                        {
                            GUI.DrawTexture(slot.rect, Tex_Aft);
                            break;
                        }
                    case QuadrantTypes.Port:
                        {
                            GUI.DrawTexture(slot.rect, Tex_Port);
                            break;
                        }
                    case QuadrantTypes.Starboard:
                        {
                            GUI.DrawTexture(slot.rect, Tex_Starboard);
                            break;
                        }
                    case QuadrantTypes.Center:
                        {
                            GUI.DrawTexture(slot.rect, Tex_Center);
                            break;
                        }
                }
            }
            else
            {
                GUI.Box(slot.rect, "");
            }
        }

        //Draw slot counts
        GUI.Label(ForeCountRect, "Fore: " + CountFore.ToString());
        GUI.Label(AftCountRect, "Aft: " + CountAft.ToString());
        GUI.Label(PortCountRect, "Port: " + CountPort.ToString());
        GUI.Label(StarboardCountRect, "Starboard: " + CountStarboard.ToString());
        GUI.Label(CenterCountRect, "Center: " + CountCenter.ToString());
        GUI.Label(TotalCountRect, "Total: " + (CountFore + CountAft + CountPort + CountStarboard + CountCenter).ToString());

        //Quadrant change button
        if (GUI.Button(QuadrantTypeButtonRect, selectedQuadrantType.ToString()))
        {
            if(selectedQuadrantType ==  QuadrantTypes.Center)
            {
                selectedQuadrantType = QuadrantTypes.Fore;
            }
            else
            {
                selectedQuadrantType++;
            }
        }
        //Reset Button
        if(GUI.Button(ResestButtonRect, "Reset"))
        {
            foreach (Slot slot in Slots)
            {
                slot.Active = false;
            }
            CountFore = 0;
            CountAft = 0;
            CountPort = 0;
            CountStarboard = 0;
            CountCenter = 0;
        }
        //Save button
        if(GUI.Button(SaveButtonRect, "Save"))
        {
            ShipSlotLayout shipSlotLayout = new ShipSlotLayout();

            foreach (Slot slot in Slots)
            {
                if (slot.Active)
                {
                    switch (slot.Quadrant)
                    {
                        case QuadrantTypes.Fore:
                            {
                                shipSlotLayout.ForeSlots.Add(slot.rect);
                                break;
                            }
                        case QuadrantTypes.Aft:
                            {
                                shipSlotLayout.AftSlots.Add(slot.rect);
                                break;
                            }
                        case QuadrantTypes.Port:
                            {
                                shipSlotLayout.PortSlots.Add(slot.rect);
                                break;
                            }
                        case QuadrantTypes.Starboard:
                            {
                                shipSlotLayout.StarboardSlots.Add(slot.rect);
                                break;
                            }
                        case QuadrantTypes.Center:
                            {
                                shipSlotLayout.CenterSlots.Add(slot.rect);
                                break;
                            }
                    }
                }
            }

            Directory.CreateDirectory(Application.dataPath + "/ShipSlotLayouts");

            string path = Application.dataPath + "/ShipSlotLayouts/" + LayoutName + ".xml";
            XmlSerializer Writer = new XmlSerializer(typeof(ShipSlotLayout));
            FileStream file = File.Create(path);
            Writer.Serialize(file, shipSlotLayout);
            file.Close();
        }

        LayoutName = GUI.TextField(NameFieldRect, LayoutName);
    }

    void ChangeCount(QuadrantTypes QT, int Num)
    {
        switch (QT)
        {
            case QuadrantTypes.Fore:
                {
                    CountFore += Num;
                    break;
                }
            case QuadrantTypes.Aft:
                {
                    CountAft += Num;
                    break;
                }
            case QuadrantTypes.Port:
                {
                    CountPort += Num;
                    break;
                }
            case QuadrantTypes.Starboard:
                {
                    CountStarboard += Num;
                    break;
                }
            case QuadrantTypes.Center:
                {
                    CountCenter += Num;
                    break;
                }
        }
    }
}

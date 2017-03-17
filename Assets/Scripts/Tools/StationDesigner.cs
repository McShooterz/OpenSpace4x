using UnityEngine;
using System.Collections.Generic;

public class StationDesigner : MonoBehaviour
{
    //Textures for different slot quadrant assignments
    public Texture2D Tex_Slot;

    public StationSlotLayout loadLayout;

    StationSlotLayout slotLayout;

    float Indent;

    List<Slot> Slots = new List<Slot>();

    //Buttons
    Rect ResestButtonRect;
    Rect SaveButtonRect;

    //Info
    Rect SlotCountRect;
    int slotCount = 0;


    // Use this for initialization
    void Start()
    {
        //Force Camera to correct position
        Camera.main.transform.position = new Vector3(0f, 5f, 0f);
        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);

        Indent = Screen.width * 0.15f;
        //Build all of the rectangles for slots
        Slots.Clear();
        for (int x = (int)Indent; x < Screen.width - 16; x += 16)
        {
            for (int y = 0; y < Screen.height - 16; y += 16)
            {
                Slots.Add(new Slot(new Rect(x, y, 16, 16)));
            }
        }

        if (loadLayout != null)
        {
            foreach (Rect loadSlot in loadLayout.SlotList)
            {
                foreach (Slot slot in Slots)
                {
                    if (loadSlot == slot.rect)
                    {
                        slot.Active = true;
                        slotCount++;
                    }
                }
            }
        }

        ResestButtonRect = new Rect(Screen.width * 0.05f, 0f, 100f, 40f);
        SaveButtonRect = new Rect(ResestButtonRect.x, ResestButtonRect.y + 60f, 100f, 40f);

        SlotCountRect = new Rect(ResestButtonRect.x, SaveButtonRect.y + 60f, 100f, 40f);
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse interactions
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            foreach (Slot slot in Slots)
            {
                if (slot.rect.Contains(mousePosition))
                {
                    if (!slot.Active)
                    {
                        slot.Active = true;
                        slotCount++;
                    }
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            foreach (Slot slot in Slots)
            {
                if (slot.rect.Contains(mousePosition) && slot.Active)
                {
                    slot.Active = false;
                    slotCount--;
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
                GUI.DrawTexture(slot.rect, Tex_Slot);
            }
            else
            {
                GUI.Box(slot.rect, "");
            }
        }

        //Draw slot counts
        GUI.Label(SlotCountRect, "Slots: " + slotCount.ToString());

        //Reset Button
        if (GUI.Button(ResestButtonRect, "Reset"))
        {
            foreach (Slot slot in Slots)
            {
                slot.Active = false;
            }
            slotCount = 0;
        }
        //Save button
        if (GUI.Button(SaveButtonRect, "Save"))
        {
            if (slotLayout == null)
            {
                //slotLayout = transform.gameObject.AddComponent<StationLayout>();
            }
            //slotLayout.SlotList.Clear();

            foreach (Slot slot in Slots)
            {
                if (slot.Active)
                {
                    //slotLayout.SlotList.Add(slot);
                }
            }
        }
    }
}

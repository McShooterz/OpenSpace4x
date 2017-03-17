/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes: 
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class CombatStoredUnitPanel
{
    Rect baseRect;
    ShipManager shipManager;
    GUIToolTip ToolTip;
    StoredUnitButton[] StoredUnitButtons = new StoredUnitButton[10];

    public CombatStoredUnitPanel(ShipManager manager, GUIToolTip toolTip)
    {
        shipManager = manager;
        ToolTip = toolTip;

        float baseRectWidth = Screen.width * 0.03f;
        baseRect = new Rect(0, Screen.height * 0.04f, baseRectWidth, baseRectWidth * 9.55f);

        float buttonSpacing = baseRect.x + baseRect.width * 0.05f;
        Vector2 ButtonSize = new Vector2(baseRect.width * 0.9f, baseRect.width * 0.9f);
        float CursorY = baseRect.y + buttonSpacing;
        for (int i = 0; i < 10; i++)
        {
            Rect ButtonRect = new Rect(new Vector2(buttonSpacing, CursorY), ButtonSize);
            CursorY += ButtonSize.y + buttonSpacing;
            StoredUnitButtons[i] = new StoredUnitButton(ButtonRect, (i + 1) % 10, shipManager);
        }
    }

    public void Draw()
    {
        GUI.Box(baseRect, "");

        foreach(StoredUnitButton button in StoredUnitButtons)
        {
            button.Draw();
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                shipManager.SelectAllShips();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(0);
                    StoredUnitButtons[9].SetIcon(shipManager.GetSelectionGroupIcon(0));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(1);
                    StoredUnitButtons[0].SetIcon(shipManager.GetSelectionGroupIcon(1));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(2);
                    StoredUnitButtons[1].SetIcon(shipManager.GetSelectionGroupIcon(2));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(3);
                    StoredUnitButtons[2].SetIcon(shipManager.GetSelectionGroupIcon(3));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(4);
                    StoredUnitButtons[3].SetIcon(shipManager.GetSelectionGroupIcon(4));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(5);
                    StoredUnitButtons[4].SetIcon(shipManager.GetSelectionGroupIcon(5));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(6);
                    StoredUnitButtons[5].SetIcon(shipManager.GetSelectionGroupIcon(6));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(7);
                    StoredUnitButtons[6].SetIcon(shipManager.GetSelectionGroupIcon(7));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(8);
                    StoredUnitButtons[7].SetIcon(shipManager.GetSelectionGroupIcon(8));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (shipManager.HasSomethingSelected())
                {
                    shipManager.StoreSelectionGroup(9);
                    StoredUnitButtons[8].SetIcon(shipManager.GetSelectionGroupIcon(9));
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            shipManager.DeselectShips();
            shipManager.RecallSelectionGroup(9);
        }
    }

    public bool Contains(Vector2 Point)
    {
        return baseRect.Contains(Point);
    }

    public bool CheckToolTip(Vector2 Point)
    {
        if(baseRect.Contains(Point))
        {
            ToolTip.SetText("selectionGroups", "selectionGroupsDesc");
            return true;
        }
        return false;
    }

    public class StoredUnitButton
    {
        Rect baseRect;
        int Index;
        ShipManager shipManager;
        Texture2D Icon;
        Rect numberRect;
        string number;

        public StoredUnitButton(Rect rect, int index, ShipManager manager)
        {
            baseRect = rect;
            Index = index;
            shipManager = manager;
            Icon = new Texture2D(1, 1);
            numberRect = new Rect(baseRect.x, baseRect.y + baseRect.height * 0.667f, baseRect.width * 0.334f, baseRect.height * 0.344f);
            number = Index.ToString();
        }

        public void Draw()
        {
            if(GUI.Button(baseRect, Icon))
            {
                shipManager.RecallSelectionGroup(Index);
            }
            GUI.Label(numberRect, number);
        }

        public void SetIcon(Texture2D icon)
        {
            Icon = icon;
        }
    }
}

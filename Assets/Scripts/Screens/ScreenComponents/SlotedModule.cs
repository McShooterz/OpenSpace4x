/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class SlotedModule
{
    public Rect baseRect;
    public Rect imageRect;
    public Module module;
    Sprite moduleTexture;
    Sprite backGroundTexture;
    public int Position;
    public float Rotation;

    public SlotedModule(Rect r, Rect i, Module mod, float rotation, int pos)
    {
        baseRect = r;
        imageRect = i;
        module = mod;
        moduleTexture = module.GetTexture();
        //backGroundTexture = ResourceManager.instance.GetUITexture("SlotedModuleBackground");

        Rotation = rotation;
        Position = pos;
    }

    public static SlotedModule CreateSlotedModule(DesignModule designModule, Rect slotRect)
    {
        Module module = ResourceManager.instance.GetModule(designModule.Module);
        if (module != null)
        {
            float moduleScale = slotRect.width;
            Rect baseRect;
            Rect imageRect = new Rect(slotRect.x, slotRect.y, module.SizeX * moduleScale, module.SizeY * moduleScale);
            if (designModule.Rotation == 0 || designModule.Rotation == 180)
            {
                baseRect = new Rect(slotRect.x, slotRect.y, module.SizeX * moduleScale, module.SizeY * moduleScale);
            }
            else
            {
                baseRect = new Rect(slotRect.x, slotRect.y, module.SizeY * moduleScale, module.SizeX * moduleScale);

                float height = moduleScale * module.SizeY;
                float width = moduleScale * module.SizeX;
                float offset = (Mathf.Max(height, width) - Mathf.Min(height, width)) / 2;
                if (height > width)
                {
                    imageRect.x += offset;
                    imageRect.y -= offset;
                }
                else
                {
                    imageRect.x -= offset;
                    imageRect.y += offset;
                }
            }
            return new SlotedModule(baseRect, imageRect, module, designModule.Rotation, designModule.Position);
        }
        return null;
    }

    public void Draw()
    {/*
        GUI.DrawTexture(baseRect, backGroundTexture);

        if (Rotation == 0)
        {
            GUI.DrawTexture(imageRect, moduleTexture);
        }
        else
        {
            Matrix4x4 matrixBackup = GUI.matrix;
            GUIUtility.RotateAroundPivot(Rotation, imageRect.center);
            GUI.DrawTexture(imageRect, moduleTexture);
            GUI.matrix = matrixBackup;
        }*/
    }

    public void SwapModule(Module newModule)
    {
        module = newModule;
        moduleTexture = module.GetTexture();
    }
}

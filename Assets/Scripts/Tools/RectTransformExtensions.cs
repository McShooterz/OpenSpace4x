/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetDefaultScale(this RectTransform rectTransform)
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public static void SetPivotAndAnchors(this RectTransform rectTransform, Vector2 aVec)
    {
        rectTransform.pivot = aVec;
        rectTransform.anchorMin = aVec;
        rectTransform.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform rectTransform)
    {
        return rectTransform.rect.size;
    }

    public static float GetWidth(this RectTransform rectTransform)
    {
        return rectTransform.rect.width;
    }

    public static float GetHeight(this RectTransform rectTransform)
    {
        return rectTransform.rect.height;
    }

    public static void SetPositionOfPivot(this RectTransform rectTransform, Vector2 newPos)
    {
        rectTransform.localPosition = new Vector3(newPos.x, newPos.y, rectTransform.localPosition.z);
    }

    public static void SetLeftBottomPosition(this RectTransform rectTransform, Vector2 newPos)
    {
        rectTransform.localPosition = new Vector3(newPos.x + (rectTransform.pivot.x * rectTransform.rect.width), newPos.y + (rectTransform.pivot.y * rectTransform.rect.height), rectTransform.localPosition.z);
    }

    public static void SetLeftTopPosition(this RectTransform rectTransform, Vector2 newPos)
    {
        rectTransform.localPosition = new Vector3(newPos.x + (rectTransform.pivot.x * rectTransform.rect.width), newPos.y - ((1f - rectTransform.pivot.y) * rectTransform.rect.height), rectTransform.localPosition.z);
    }

    public static void SetRightBottomPosition(this RectTransform rectTransform, Vector2 newPos)
    {
        rectTransform.localPosition = new Vector3(newPos.x - ((1f - rectTransform.pivot.x) * rectTransform.rect.width), newPos.y + (rectTransform.pivot.y * rectTransform.rect.height), rectTransform.localPosition.z);
    }

    public static void SetRightTopPosition(this RectTransform rectTransform, Vector2 newPos)
    {
        rectTransform.localPosition = new Vector3(newPos.x - ((1f - rectTransform.pivot.x) * rectTransform.rect.width), newPos.y - ((1f - rectTransform.pivot.y) * rectTransform.rect.height), rectTransform.localPosition.z);
    }

    public static void SetSize(this RectTransform rectTransform, Vector2 newWidthHeight)
    {
        Vector2 oldSize = rectTransform.rect.size;
        Vector2 deltaSize = newWidthHeight - oldSize;
        rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(deltaSize.x * rectTransform.pivot.x, deltaSize.y * rectTransform.pivot.y);
        rectTransform.offsetMax = rectTransform.offsetMax + new Vector2(deltaSize.x * (1f - rectTransform.pivot.x), deltaSize.y * (1f - rectTransform.pivot.y));
    }

    public static void SetWidth(this RectTransform rectTransform, float newSize)
    {
        SetSize(rectTransform, new Vector2(newSize, rectTransform.rect.size.y));
    }

    public static void SetHeight(this RectTransform rectTransform, float newSize)
    {
        SetSize(rectTransform, new Vector2(rectTransform.rect.size.x, newSize));
    }
}

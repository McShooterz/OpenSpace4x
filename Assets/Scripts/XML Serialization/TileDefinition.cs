using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDefinition
{
    public string displayName;

    public string[] images;

    public BonusEntry[] potentialBonuses;

	public TileDefinition()
    {

    }

    public string GetDisplayName()
    {
        return ResourceManager.instance.GetLocalization(displayName);
    }

    public Sprite GetImage(int index)
    {
        if (images.Length > 0)
        {
            index = Mathf.Clamp(index, 0, images.Length);

            return ResourceManager.instance.GetPlanetTileImage(images[index]);
        }
        else
        {
            return ResourceManager.instance.GetErrorTexture();
        }
    }

    public int GetRandomImageIndex()
    {
        if (images.Length > 0)
        {
            return Random.Range(0, images.Length);
        }
        else
        {
            return 0;
        }
    }

    public class BonusEntry
    {
        public TileBonusType bonus;

        public float weight;
    }
}

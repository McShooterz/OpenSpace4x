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

    public BonusEntry GetRandomTileBonus()
    {
        if (potentialBonuses.Length > 0)
        {
            if (potentialBonuses.Length == 1)
            {
                return potentialBonuses[0];
            }
            else
            {
                float[] weights = new float[potentialBonuses.Length];

                for(int i = 0; i < potentialBonuses.Length; i++)
                {
                    weights[i] = potentialBonuses[i].weight;
                }

                return potentialBonuses[StaticHelpers.GetRandomIndexByWeight(weights)];
            }
        }

        return null;
    }

    public class BonusEntry
    {
        public TileBonusType bonus;

        public float value;

        public float weight;
    }
}

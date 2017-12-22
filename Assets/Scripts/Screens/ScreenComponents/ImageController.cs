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
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    Image image;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public void SetTexture(Texture2D texture)
    {
        image.overrideSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void SetSize(Vector2 size)
    {
        rectTransform.SetSize(size);
    }

    public void SetPosition(Vector2 position)
    {
        position.x += rectTransform.rect.width / 2f;
        position.y -= rectTransform.rect.height / 2f;
        rectTransform.position = position;
    }
}

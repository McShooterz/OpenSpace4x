/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class PopupMessage : MonoBehaviour
{
    float LifeSpan = 3f;
    float RiseRate = 1f;
    TextMesh textMesh;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LifeSpan > 0)
        {
            LifeSpan -= Time.deltaTime;
            transform.position += Vector3.up * RiseRate * Time.deltaTime;
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, LifeSpan);
        }
        else
        {
            DestroySelf();
        }       
    }

    public void Initialize(float lifeTime, float riseRate)
    {
        LifeSpan = lifeTime;
        RiseRate = riseRate;
        gameObject.AddComponent<MeshRenderer>();
        textMesh = gameObject.AddComponent<TextMesh>();
    }

    public TextMesh GetTextMesh()
    {
        return textMesh;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}

/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public sealed class CreditsScreen : BaseScreen
{
    [SerializeField]
    Text GameTitle;
    [SerializeField]
    Text Header;
    [SerializeField]
    Text Entry;

    List<Text> creditItems = new List<Text>();

    [SerializeField]
    float ScrollSpeed = 1f;

    void Start()
    {
        creditItems.Add(GameTitle);

        float GroupSpacing = GameTitle.rectTransform.position.y - Header.rectTransform.position.y;
        float EntrySpacing = Header.rectTransform.position.y - Entry.rectTransform.position.y;

        float XPosition = Header.rectTransform.position.x;
        float YPosition = Header.rectTransform.position.y;

        foreach (Credits.CreditsGroup group in ResourceManager.instance.GetCredits().CreditGroups)
        {
            Text GroupHeader = Instantiate(Header.gameObject).GetComponent<Text>();
            GroupHeader.text = group.GroupName;
            YPosition -= EntrySpacing;
            Vector2 Position = new Vector2(XPosition, YPosition);
            GroupHeader.rectTransform.position = Position;
            GroupHeader.rectTransform.SetParent(GameTitle.rectTransform.parent);
            creditItems.Add(GroupHeader);
            foreach (string entry in group.GroupItems)
            {
                Text GroupEntry = Instantiate(Entry.gameObject).GetComponent<Text>();
                GroupEntry.text = entry;
                YPosition -= EntrySpacing;
                Position = new Vector2(XPosition, YPosition);
                GroupEntry.rectTransform.position = Position;
                GroupEntry.rectTransform.SetParent(GameTitle.rectTransform.parent);
                creditItems.Add(GroupEntry);
            }
            YPosition -= GroupSpacing;
        }

        Destroy(Header.gameObject);
        Destroy(Entry.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            ChangeLastScreen();
        }

        foreach (Text item in creditItems)
        {
            Vector3 Position = item.rectTransform.position;
            Position = new Vector2(Position.x, Position.y + ScrollSpeed);
            item.rectTransform.position = Position;
        }
    }
}

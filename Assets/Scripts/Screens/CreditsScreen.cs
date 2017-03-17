using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class CreditsScreen : ScreenParent
{
    List<creditItem> creditItems = new List<creditItem>();

    float ScrollSpeed = -1f;

    public CreditsScreen()
    {
        Vector2 TitleSize = new Vector2(Screen.width * 0.1f, Screen.height * 0.06f);
        Vector2 EntrySize = new Vector2(Screen.width * 0.08f, Screen.height * 0.05f);
        float TitleIndent = (Screen.width - TitleSize.x) / 2f;
        float EntryIndent = (Screen.width - EntrySize.x) / 2f;
        float GroupSpacing = Screen.height * 0.15f;
        float cursorY = Screen.height;

        Credits credits = ResourceManager.instance.GetCredits();

        foreach(Credits.CreditsGroup group in credits.CreditGroups)
        {
            creditItems.Add(new creditItem(new Rect(TitleIndent, cursorY, TitleSize.x, TitleSize.y), group.GroupName, GameManager.instance.CreditsTitleStyle));
            cursorY += TitleSize.y;
            foreach (string entry in group.GroupItems)
            {
                creditItems.Add(new creditItem(new Rect(EntryIndent, cursorY, EntrySize.x, EntrySize.y), entry, GameManager.instance.CreditsEntryStyle));
                cursorY += EntrySize.y;
            }
            cursorY += GroupSpacing;
        }
    }

    public override void Update()
    {
        if (Input.anyKeyDown)
        {
            CloseScreen();
        }

        foreach (creditItem item in creditItems)
        {
            item.Scroll(ScrollSpeed);
        }
    }

    public override void Draw()
    {
        foreach (creditItem item in creditItems)
        {
            item.Draw();
        }
    }

    protected override void CloseScreen()
    {
        GameManager.instance.ChangeScreen(new MainMenuScreen());
    }

    class creditItem
    {
        Rect rect;
        string name;
        GUIStyle style;

        public creditItem(Rect r, string n, GUIStyle s)
        {
            rect = r;
            name = n;
            style = s;
        }

        public void Draw()
        {
            GUI.Label(rect, name, style);
        }

        public void Scroll(float amount)
        {
            rect.y += amount;
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystemController : MonoBehaviour
{
    [SerializeField]
    CampaignMainScreen parentScreen;

    [SerializeField]
    Image playIcon;

    [SerializeField]
    Image pauseIcon;

    [SerializeField]
    Text stardateText;

    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Stardate stardate;

    public static float timeModifier = 1f;

    public static bool timePaused = true;

    float[] timeIncrements = new float[5] { 0.25f, 0.5f, 1.0f, 1.5f, 2.0f};
    int timeIndex = 2;

    float currentDayTime = 0f;

	// Use this for initialization
	void Start ()
    {
        SetPaused();
        SetStardateText();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }

		if (!timePaused)
        {
            currentDayTime += Time.deltaTime * timeModifier;

            if (currentDayTime > 1f)
            {
                currentDayTime -= 1f;
                EmpireManager.instance.ChangeDay();
                if (stardate.AddDay())
                {
                    EmpireManager.instance.ChangeMonth();
                    parentScreen.ChangeMonth();   
                }
                SetStardateText();
            }
        }
	}

    void SetPaused()
    {
        timePaused = true;
        playIcon.gameObject.SetActive(false);
        pauseIcon.gameObject.SetActive(true);

        descriptionText.text = "Paused";
    }

    void SetUnpaused()
    {
        timePaused = false;
        playIcon.gameObject.SetActive(true);
        pauseIcon.gameObject.SetActive(false);

        SetTimeDescription(timeIndex);
    }

    public void ClickPlayPauseButton()
    {
        TogglePause();
    }

    void TogglePause()
    {
        if (timePaused)
        {
            SetUnpaused();
        }
        else
        {
            SetPaused();
        }
    }

    public void ClickSpeedIncrease()
    {
        if (timeIndex < 4)
        {
            timeIndex++;
            if (!timePaused)
            {
                SetTimeDescription(timeIndex);
                SetTimeModifier(timeIndex);
            }
        }
    }

    public void ClickSpeedDecrease()
    {
        if (timeIndex > 0)
        {
            timeIndex--;
            if (!timePaused)
            {
                SetTimeDescription(timeIndex);
                SetTimeModifier(timeIndex);
            }
        }
    }

    void SetTimeModifier(int index)
    {
        timeModifier = timeIncrements[index];
    }

    void SetTimeDescription(int index)
    {
        switch (index)
        {
            case 0:
                {
                    descriptionText.text = "Slowest";
                    break;
                }
            case 1:
                {
                    descriptionText.text = "Slow";
                    break;
                }
            case 2:
                {
                    descriptionText.text = "Normal";
                    break;
                }
            case 3:
                {
                    descriptionText.text = "Fast";
                    break;
                }
            case 4:
                {
                    descriptionText.text = "Fastest";
                    break;
                }
        }
    }

    void SetStardateText()
    {
        stardateText.text = stardate.GetYear().ToString() + "." + stardate.GetMonth().ToString("D2") + "." + stardate.GetDay().ToString("D2");
    }
}

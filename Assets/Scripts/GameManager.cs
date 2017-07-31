using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum Mood { Interested, Bored, Confused };

    Mood[] Audience;
    bool[] moodChangedThisFrame;

    [System.NonSerialized]
    public float Timer = 0;

    [SerializeField]
    Camera[] views;

    public int Interest {
        get
        {
            int result = 0;
            foreach (Mood m in Audience)
            {
                if (m == Mood.Interested)
                {
                    result++;
                }
            }
            return result;
        }
    }
    public int Boredom
    {
        get
        {
            int result = 0;
            foreach (Mood m in Audience)
            {
                if (m == Mood.Bored)
                {
                    result++;
                }
            }
            return result;
        }
    }
    public int Confusion
    {
        get
        {
            int result = 0;
            foreach (Mood m in Audience)
            {
                if (m == Mood.Confused)
                {
                    result++;
                }
            }
            return result;
        }
    }

    float deadTime;
    float pauseTime;
    float timeSinceLastBoredomIncrease;
    
    [SerializeField]
    float chanceOfInterestPerElement;
    [SerializeField]
    float chanceOfInterestFromTalk;
    [SerializeField]
    float chanceOfInterestPerRepeat;

    [SerializeField]
    float checkBoredomFreq;
    [SerializeField]
    float chanceOfBored;
    [SerializeField]
    float chanceOfBoredPerRepeat;
    [SerializeField]
    float chanceOfBoredFromEmpty;
    [SerializeField]
    float boredomAccelerationTime;
    [SerializeField]
    float boredomAccelerationAmount;
    
    [SerializeField]
    float chanceOfConfusedPerOverlap;
    [SerializeField]
    float chanceOfConfusedFromEmpty;
    [SerializeField]
    float chanceOfConfusedPerSkippedElement;
    [SerializeField]
    float confusionBoredomPenalty;


    void Awake()
    {
        Game.Manager = this;
        Audience = new Mood[100];
        moodChangedThisFrame = new bool[100];
    }

    private void Start()
    {
        StartCoroutine(BoredomCheck());
        timeSinceLastBoredomIncrease = 0;
        Timer = 0;
    }

    void Update()
    {
        Timer += Time.deltaTime;
        timeSinceLastBoredomIncrease += Time.deltaTime;
        if (timeSinceLastBoredomIncrease > boredomAccelerationTime)
        {
            chanceOfBored *= boredomAccelerationAmount;
            chanceOfBoredPerRepeat *= boredomAccelerationAmount;
            chanceOfBoredFromEmpty *= boredomAccelerationAmount;
            timeSinceLastBoredomIncrease = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeView(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeView(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeView(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Interest + Confusion - Boredom <= 0)
        {
            this.enabled = false;
            Game.Speech.enabled = false;
            StopAllCoroutines();
            Game.Powerpoint.GetComponentInChildren<Button>().interactable = false;
            Game.Menu.Invoke("GameOver", 1);
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < moodChangedThisFrame.Length; i++)
        {
            moodChangedThisFrame[i] = false;
        }
    }

    public void ChangeView(int index)
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].depth = i == index ? 0 : -1;
        }
    }

    public void PauseBoredom(float duration)
    {
        pauseTime = duration;
    }

    IEnumerator BoredomCheck()
    {
        while (true)
        {
            if (pauseTime > 0)
            {
                pauseTime -= Time.deltaTime;
            }
            else
            {
                deadTime += Time.deltaTime;
                if (deadTime > checkBoredomFreq)
                {
                    ChangeAudienceMood(Mood.Bored, chanceOfBored);
                    deadTime = 0;
                }
            }
            yield return null;
        }
    }

    public void NewSlideMood()
    {
        ChangeAudienceMood(Mood.Interested, chanceOfInterestPerElement * Game.Slideshow.Variety);
        ChangeAudienceMood(Mood.Confused, chanceOfConfusedPerOverlap * Game.Slideshow.OverlapRating);
        if (Game.Slideshow.clickables.Count == 0)
        {
            ChangeAudienceMood(Mood.Confused, chanceOfConfusedFromEmpty);
            ChangeAudienceMood(Mood.Bored, chanceOfBoredFromEmpty);
        }
    }

    public void EndSlideMood()
    {
        int skipCount = 0;
        foreach (Slideshow.Clickable cl in Game.Slideshow.clickables)
        {
            if (cl.numTimesSelected == 0)
                skipCount++;
        }
        ChangeAudienceMood(Mood.Confused, chanceOfConfusedPerSkippedElement * skipCount);
    }

    public void FinishTalkMood()
    {
        ChangeAudienceMood(Mood.Bored, chanceOfBoredPerRepeat * (Game.Speech.talkTarget.numTimesSelected - 1));
        ChangeAudienceMood(Mood.Interested, chanceOfInterestFromTalk + (Game.Speech.talkTarget.numTimesSelected-1) * chanceOfInterestPerRepeat);
    }

    public void ChangeAudienceMood(Mood mood, float chance)
    {
        Debug.Log("Chance of " + mood + ": " + chance);

        for (int i = 0; i < Audience.Length; i++)
        {
            if (mood == Mood.Confused && Audience[i] == Mood.Bored)
                continue;

            float modChance = mood == Mood.Bored && Audience[i] == Mood.Confused ? chance * confusionBoredomPenalty : chance;

            if (!moodChangedThisFrame[i] && Random.value < modChance)
            {
                moodChangedThisFrame[i] = true;
                Audience[i] = mood;
                switch(mood)
                {
                    case Mood.Interested:
                        Game.MoodDisplay.InterestedThisFrame++;
                        break;
                    case Mood.Confused:
                        Game.MoodDisplay.ConfusedThisFrame++;
                        break;
                    case Mood.Bored:
                        Game.MoodDisplay.BoredThisFrame++;
                        break;
                }
            }
        }
    }
}

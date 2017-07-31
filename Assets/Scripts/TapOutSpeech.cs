using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TapOutSpeech : MonoBehaviour {

    [SerializeField]
    Animator character;
    [SerializeField]
    GameObject[] speechBubbles;
    [SerializeField]
    Text[] speeches;

    [SerializeField]
    int maxChars;
    [SerializeField]
    string[] businessWords;
    [SerializeField]
    string fillerWord;

    List<string> words = new List<string>();
    [System.NonSerialized]
    public Slideshow.Clickable talkTarget = null;

    private void Awake()
    {
        Game.Speech = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && talkTarget != null)
        {
            if (words.Count > 0)
            {
                foreach (GameObject speechBubble in speechBubbles)
                    speechBubble.SetActive(true);

                foreach (Text speech in speeches)
                    speech.text += speech.text.Length == 0 ? words[0] : " " + words[0];

                words.RemoveAt(0);
                character.SetTrigger("Speak");

                Game.Manager.PauseBoredom(0.5f);

                Game.Audio.Talk();
            }
            else
            {
                Game.Manager.FinishTalkMood();
                talkTarget = null;
                foreach (GameObject speechBubble in speechBubbles)
                    speechBubble.SetActive(false);
            }
        }

    }

    public void StartTalk(Slideshow.Clickable t)
    {
        if (talkTarget != null)
            return;

        t.numTimesSelected++;
        talkTarget = t;

        foreach (GameObject speechBubble in speechBubbles)
            speechBubble.SetActive(true);

        foreach (Text speech in speeches)
            speech.text = "";
        GenerateText();
    }

    public void GenerateText()
    {
        string word1 = businessWords[Random.Range(0, businessWords.Length - 1)];

        string textToShow = "";

        while (textToShow.Length < maxChars)
        {
            string textToAdd = "";
            bool textSet = false;
            if (Random.value <= 0.85f && !textSet)
            {
                textToAdd = fillerWord;
                textSet = textToAdd.Length + textToShow.Length <= maxChars;
            }
            
            if (!textSet)
            {
                textToAdd = word1;
                textSet = textToAdd.Length + textToShow.Length <= maxChars;
            }

            if (!textSet)
            {
                textToAdd = fillerWord;
                textSet = textToAdd.Length + textToShow.Length <= maxChars;
            }

            if (textSet)
            {
                textToShow += textToAdd + " ";
            }
            else
            {
                break;
            }
        }

        textToShow = textToShow.Trim();
        words = new List<string>(textToShow.Split(' '));
    }
}

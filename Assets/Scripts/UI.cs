using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    [SerializeField]
    Text[] InterestFields;
    [SerializeField]
    Text[] TimerFields;

    [SerializeField]
    RectTransform interestMeter;
    [SerializeField]
    RectTransform confusionMeter;
    [SerializeField]
    RectTransform boredomMeter;

    private void Awake()
    {
        Game.UI = this;
    }

    void Update ()
    {
        foreach (Text InterestField in InterestFields)
        {
            InterestField.text = (Game.Manager.Interest + Game.Manager.Confusion - Game.Manager.Boredom).ToString();
        }
        foreach (Text TimerField in TimerFields)
        {
            TimerField.text = Mathf.FloorToInt(Game.Manager.Timer / 60) + ":" + Mathf.FloorToInt(Game.Manager.Timer % 60).ToString("00");
        }

        Vector2 size = Vector2.zero;
        size.x = Mathf.Round(Game.Manager.Interest / 2.5f) * 2;
        interestMeter.sizeDelta = size;
        size.x = Mathf.Round(Game.Manager.Confusion / 2.5f) * 2;
        confusionMeter.sizeDelta = size;
        size.x = Mathf.Round(Game.Manager.Boredom / 2.5f) * 2;
        boredomMeter.sizeDelta = size;
    }
}

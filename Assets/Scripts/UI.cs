using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    [SerializeField]
    Text InterestField;
    [SerializeField]
    Text TimerField;
    
    void Update () 
    {
        InterestField.text = Mathf.CeilToInt(Game.Manager.Interest).ToString();
        TimerField.text = Mathf.FloorToInt(Game.Manager.Timer / 60) + ":" + Mathf.CeilToInt(Game.Manager.Timer % 60).ToString("00");
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoodDisplay : MonoBehaviour {

    [SerializeField]
    List<Animator> display0;
    [SerializeField]
    List<Animator> display1;
    [SerializeField]
    List<Animator> display2;

    [System.NonSerialized]
    public int BoredThisFrame;
    [System.NonSerialized]
    public int ConfusedThisFrame;
    [System.NonSerialized]
    public int InterestedThisFrame;

    private void Awake()
    {
        Game.MoodDisplay = this;
    }

    private void LateUpdate()
    {
        if (BoredThisFrame > 0)
            Game.Audio.Bored();
        if (ConfusedThisFrame > 0)
            Game.Audio.Confused();
        if (InterestedThisFrame > 0)
            Game.Audio.Interested();

        for (int i = 0; i < BoredThisFrame; i++)
        {
            if (i < display0.Count)
                display0.RandomItem().SetTrigger("Bored");
            if (i < display1.Count)
                display1.RandomItem().SetTrigger("Bored");
            if (i < display2.Count)
                display2.RandomItem().SetTrigger("Bored");
        }
        for (int i = 0; i < ConfusedThisFrame; i++)
        {
            if (i < display0.Count)
                display0.RandomItem().SetTrigger("Confused");
            if (i < display1.Count)
                display1.RandomItem().SetTrigger("Confused");
            if (i < display2.Count)
                display2.RandomItem().SetTrigger("Confused");
        }
        for (int i = 0; i < InterestedThisFrame; i++)
        {
            if (i < display0.Count)
                display0.RandomItem().SetTrigger("Interested");
            if (i < display1.Count)
                display1.RandomItem().SetTrigger("Interested");
            if (i < display2.Count)
                display2.RandomItem().SetTrigger("Interested");
        }
        BoredThisFrame = 0;
        ConfusedThisFrame = 0;
        InterestedThisFrame = 0;
    }
}

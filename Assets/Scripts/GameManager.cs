using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [System.NonSerialized]
    public float Interest;

    [SerializeField]
    float startingInterest;
    [SerializeField]
    float interestDecay;
    [SerializeField]
    float interestDecayIncreaseMult;
    [SerializeField]
    float interestDecayIncreaseTime;

    [SerializeField]
    float interestPerWord;

    [System.NonSerialized]
    public float Timer = 0;
    float timeSinceLastDecayIncrease = 0;
    

    void Awake () 
    {
        Game.Manager = this;
    }

    void Start()
    {
        Interest = startingInterest;
    }

    void Update () 
    {
        Timer += Time.deltaTime;
        Interest -= interestDecay * Time.deltaTime;
        timeSinceLastDecayIncrease += Time.deltaTime;
        if (timeSinceLastDecayIncrease > interestDecayIncreaseTime)
        {
            timeSinceLastDecayIncrease = 0;
            interestDecay *= interestDecayIncreaseMult;
        }
    }

    public void WordSaid()
    {
        Interest += interestPerWord;
    }
}

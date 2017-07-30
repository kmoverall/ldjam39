using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    GameObject Tint;
    [SerializeField]
    GameObject TitleScreen;
    [SerializeField]
    GameObject GameOverScreen;
    [SerializeField]
    Text GameOverTimeText;

    [SerializeField]
    GameObject[] HelpScreens;
    [SerializeField]
    int[] HelpViews;

    int currentHelp;

    private void Awake()
    {
        Game.Menu = this;
    }

    void Start () 
    {
        Tint.SetActive(true);
        TitleScreen.SetActive(true);
    }
    
    void Update () {
        if (Game.Manager.enabled)
            return;

        if (TitleScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TitleScreen.SetActive(false);
                Tint.SetActive(false);
                Game.Manager.enabled = true;
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                TitleScreen.SetActive(false);
                HelpScreens[0].SetActive(true);
                Game.Manager.ChangeView(HelpViews[0]);
                currentHelp = 0;
            }
            return;
        }
        if (GameOverScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Game");
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HelpScreens[currentHelp].SetActive(false);

            currentHelp++;

            if (currentHelp == HelpScreens.Length)
            {
                TitleScreen.SetActive(true);
                Game.Manager.ChangeView(0);
                return;
            }

            HelpScreens[currentHelp].SetActive(true);
            Game.Manager.ChangeView(HelpViews[currentHelp]);
            return;
        }
    }

    public void GameOver()
    {
        GameOverTimeText.text = "you presented for " + Mathf.FloorToInt(Game.Manager.Timer / 60) + ":" + Mathf.CeilToInt(Game.Manager.Timer % 60).ToString("00") + "!";
        Tint.SetActive(true);
        GameOverScreen.SetActive(true);
    }
}

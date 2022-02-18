using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.EventBehaviour;
using Coskunerov.EventBehaviour.Attributes;
using Coskunerov.Actors;
using TMPro;
using Coskunerov.Managers;

public class UIActor : GameSingleActor<UIActor>
{
    public GameObject InGameWindow;
    public GameObject WinGameWindow;
    public GameObject LoseGameWindow;
    public GameObject Tutorial;


    public void ButtonClick(int id)
    {
        switch (id)
        {
            case 0:
                GameManager.Instance.RestartLevel();
                break;
            case 1:
                GameManager.Instance.NextLevel();
                break;
        }
    }
    [GE(BaseGameEvents.LevelLoaded)]
    public void LoadGame()
    {
        WinGameWindow.SetActive(false);
        LoseGameWindow.SetActive(false);
        InGameWindow.SetActive(true);
    }
    [GE(BaseGameEvents.WinGame)]
    public void WinGame()
    {
        InGameWindow.SetActive(false);
        WinGameWindow.SetActive(true);
    }
    [GE(BaseGameEvents.LoseGame)] 
    public void LoseGame()
    {
        Debug.Log("Fail");
        InGameWindow.SetActive(false);
        LoseGameWindow.SetActive(true);
    }

    [GE(2000)] 
    public void ShowTutorial()
    {
        Tutorial.gameObject.SetActive(true);
    }
    [GE(2001)]
    public void HideTutorial()
    {
        Tutorial.gameObject.SetActive(false);
    }
}

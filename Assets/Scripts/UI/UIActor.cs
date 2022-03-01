using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.EventBehaviour;
using Coskunerov.EventBehaviour.Attributes;
using Coskunerov.Actors;
using TMPro;
using Coskunerov.Managers;
using UnityEngine.UI;
using DG.Tweening;

public class UIActor : GameSingleActor<UIActor>
{
    public GameObject InGameWindow;
    public Text ActionText;
    public List<string> ActionTextList;
    public GameObject WinGameWindow;
    public GameObject LoseGameWindow;
    public GameObject Tutorial;

    public GameObject bigButton;
    public Text tabToStartText;
    Tween tabToStartTextTween=null;

    public override void ActorAwake()
    {
        tabToStartTextTween = DOTween.Sequence().Append(tabToStartText.transform.DOScale(tabToStartText.transform.localScale / 1.2f, 0.6f)).SetLoops(-1,LoopType.Yoyo);
    }

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
            case 2:
                GameManager.Instance.PushEvent(4000);
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
    [GE(3000)]
    public void ShowActionText()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            ActionText.text = ActionTextList[Random.Range(0,ActionTextList.Count)];
            ActionText.gameObject.SetActive(true);
            ActionText.transform.DOPunchScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(2);
            ActionText.gameObject.SetActive(false);
        }
    }
    [GE(3001)]
    public void ShowActionTextFever()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            ActionText.text = "Fever Mode!";
            ActionText.gameObject.SetActive(true);
            ActionText.transform.DOPunchScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(2);
            ActionText.gameObject.SetActive(false);
        }
    }
    [GE(4000)]
    public void OnGameStartTabClicked()
    {
        bigButton.SetActive(false);
        if (tabToStartTextTween!=null)
        {
            tabToStartTextTween.Kill();
        }
    }



}

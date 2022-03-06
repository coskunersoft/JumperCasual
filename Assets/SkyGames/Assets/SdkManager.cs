using GameAnalyticsSDK;
using UnityEngine;

public class SdkManager : MonoBehaviour
{
    // public GameObject winPanel, failPanel;
    public static SdkManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        GameAnalytics.Initialize();
    }
    private void Start()
    {
        TinySauce.OnGameStarted(PlayerPrefs.GetInt("Level").ToString());
    }

    public void Win()
    {
        // winPanel.SetActive(true);
        print(PlayerPrefs.GetInt("Level"));
        TinySauce.OnGameFinished(true, 0, "" + PlayerPrefs.GetInt("Level"));
    }
    public void Fail()
    {
        // failPanel.SetActive(true);
        print(PlayerPrefs.GetInt("Level"));
        TinySauce.OnGameFinished(false, 0, "" + PlayerPrefs.GetInt("Level"));
    }

    public void NextLevel()
    {
        Debug.Log("Next Button");
    }

    public void Retry()
    {
        Debug.Log("Retry Button");
    }

}

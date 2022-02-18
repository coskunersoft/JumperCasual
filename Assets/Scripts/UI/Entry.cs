using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Entry : MonoBehaviour
{
  

    public void OnClicked()
    {
        SceneManager.LoadScene("Game");
    }

}

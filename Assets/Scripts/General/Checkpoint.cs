using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, ITriggerListener
{
    public void OnTriggerEnterListen(MonoBehaviour toucher)
    {
        if (toucher is Jumper jumper)
        {
            jumper.UpdateCheckPoint(transform);
        }
    }
}

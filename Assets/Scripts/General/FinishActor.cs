using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishActor : MonoBehaviour , ITriggerListener
{
    private Collider col;
    private void Awake()
    {
        gameObject.TryGetComponent(out col);
    }
    public void OnTriggerEnterListen(MonoBehaviour toucher)
    {
        if (toucher is Jumper jumper)
        {
            jumper.OnTouchedFinish(this);
            if (col)
                col.enabled = false;
        }
    }



}

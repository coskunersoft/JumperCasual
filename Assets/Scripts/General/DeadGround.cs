using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadGround : MonoBehaviour,ITriggerListener
{
  
    public void OnTriggerEnterListen(MonoBehaviour toucher)
    {
        if (toucher is Jumper jumper)
        {
            jumper.OnTouchedDeadGround(this);
        }
    }
}

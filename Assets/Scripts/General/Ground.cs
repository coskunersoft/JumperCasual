using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour, ICollisionExitListerner, ICollisionEnterListerner
{
    public void OnCollisionExitListen(MonoBehaviour toucher)
    {
        if (toucher is Jumper jumper)
        {
            jumper.OnExitGround(this);
        }
    }

    public void OnCollisionEnterListen(MonoBehaviour toucher)
    {
        if(toucher is Jumper jumper)
        {
            jumper.OnEnterGround(this);
        }
    }
}

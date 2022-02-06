using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour, ICollisionExitListerner, ICollisionEnterListerner
{
    public void OnCollisionExitListen(MonoBehaviour toucher)
    {
        if (toucher is Player player)
        {
            player.OnExitGround(this);
        }
    }

    public void OnCollisionEnterListen(MonoBehaviour toucher)
    {
        if(toucher is Player player)
        {
            player.OnEnterGround(this);
        }
    }
}

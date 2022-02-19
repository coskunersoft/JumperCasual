using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPoint : MonoBehaviour ,ITriggerListener
{
    public PointType pointType;
    public List<BotPointValue> BotPointValue;

    public void OnTriggerEnterListen(MonoBehaviour toucher)
    {
        if (toucher is BotPlayer botPlayer)
        {

        }
    }


    public enum PointType
    {
        JumpTrigger , 
    }
}

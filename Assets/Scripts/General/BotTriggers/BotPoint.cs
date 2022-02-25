using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPoint : MonoBehaviour ,ITriggerListener
{
    public PointType pointType;
    public List<BotPointValue> BotPointValue;

    private Collider col;
    private void Awake()
    {
        gameObject.TryGetComponent(out col);
    }

    public void OnTriggerEnterListen(MonoBehaviour toucher)
    {
        if (toucher is BotPlayer botPlayer)
        {
            botPlayer.TouchBotPoint(this);
            StartCoroutine(DelayedDisable());
        }
    }

    IEnumerator DelayedDisable()
    {
        if (!col) yield break;

            col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }


    public enum PointType
    {
        JumpTrigger , 
    }
}

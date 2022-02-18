using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadGround : MonoBehaviour,ITriggerListener
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
            jumper.OnTouchedDeadGround(this);
            if (col)
                col.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionExitListerner
{
    public void OnCollisionExitListen(MonoBehaviour toucher);
}
public interface ICollisionEnterListerner
{
    public void OnCollisionEnterListen(MonoBehaviour toucher);
}

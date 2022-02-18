using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour,ITriggerListener
{
    public BlockType blockType;
    private Collider col;
    private void Awake()
    {
        gameObject.TryGetComponent(out col);
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * 360 * Time.deltaTime);
    }

    public void OnTriggerEnterListen(MonoBehaviour toucher)
    {
        if (toucher is Jumper player)
        {
            player.OnTouchBlock(this);
            if (col)
                col.enabled = false;
        }
    }

    public enum BlockType
    {
        Saw
    }
}

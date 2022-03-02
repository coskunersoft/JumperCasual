using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour,ITriggerListener
{
    public BlockType blockType;
 

    private void Update()
    {
        if(blockType==BlockType.Saw)
        transform.Rotate(Vector3.forward * 360 * Time.deltaTime);
    }

    public void OnTriggerEnterListen(MonoBehaviour toucher)
    {
        if (toucher is Jumper player)
        {
            player.OnTouchBlock(this);
        
        }
    }

    public enum BlockType
    {
        Saw,Static
    }
}

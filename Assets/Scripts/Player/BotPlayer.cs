using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : Jumper
{
    

    public override void ActorUpdate()
    {
        base.ActorUpdate();
        MovementController();
    }

    private void BotJump()
    {
        Jump();
    }


    public void TouchBotPoint(BotPoint botPoint)
    {
        switch (botPoint.pointType)
        {
            case BotPoint.PointType.JumpTrigger:
                BotJump();
                break;
        }
    }

    public enum State
    {
        Moving,Jumping
    }
}

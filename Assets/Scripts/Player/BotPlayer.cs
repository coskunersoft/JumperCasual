using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Coskunerov.Managers;

public class BotPlayer : Jumper
{
    public static bool isFinished = false;

    public override void ActorAwake()
    {
        isFinished = false;
        base.ActorAwake();
    }

    public override void ActorUpdate()
    {
        base.ActorUpdate();
        MovementController();
    }

    private void BotJump(BotPoint botPoint)
    {
        var value=botPoint.BotPointValue.FirstOrDefault(x => x is BPJumpValue) as BPJumpValue;
        if (value==null) return;
        DOTween.To(() => strectAmount, x => strectAmount = x, value.GetValue, value.ReadyTime).SetEase(Ease.InOutBounce).OnComplete(() =>
          {
              Jump();
          }).OnUpdate(()=>
          {
              SkinnedMeshRenderer.SetBlendShapeWeight(0, (strectAmount / 5f) * 100);
          });
    }

    public void TouchBotPoint(BotPoint botPoint)
    {
        switch (botPoint.pointType)
        {
            case BotPoint.PointType.JumpTrigger:
                BotJump(botPoint);
                break;
        }
    }


    protected override IEnumerator WinSquence()
    {
        yield return base.WinSquence();
        isFinished = true;
        movementSpeed = 0;
        rb.velocity = Vector3.zero;
    }


    public enum State
    {
        Moving,Jumping
    }
}

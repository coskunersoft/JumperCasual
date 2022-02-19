using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Coskunerov.Tools;
using Sirenix.OdinInspector;
using DG.Tweening;
using Coskunerov.Managers;

public class Player : Jumper
{
    public static Player Instance;
    protected InputManager inputManager;

    public override void ActorAwake()
    {
        base.ActorAwake();
        Instance = this;
        inputManager = GetComponent<InputManager>();
    }

    public override void ActorUpdate()
    {
        if (isDead) return;
        base.ActorUpdate();
        TabController();
    } 
   

    public override void ActorFixedUpdate()
    {
        if (isDead) return;
        base.ActorFixedUpdate();
        MovementController();
    }

    private void MovementController()
    {
        if(isGrounded)
        rb.velocity = new Vector3(0, rb.velocity.y,movementSpeed);
    }

    private void TabController()
    {
        if (Input.GetMouseButton(0))
        {
            strecing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            strecing = false;
            Jump();
        }
        if (!isGrounded) return;
        if (strecing)
        {
            strectAmount += Time.deltaTime*strectSpeed;
            strectAmount = Mathf.Clamp(strectAmount,0, 5f);
            SkinnedMeshRenderer.SetBlendShapeWeight(0, (strectAmount / 5f) * 100);
        }
       
    }

    protected override void Dead(DeadType deadType)
    {
        base.Dead(deadType);
        GameManager.Instance.FinishLevel(false);
    }

    private IEnumerator Win()
    {
        gameObject.SetActive(false);
        GameManager.Instance.FinishLevel(true);
        yield return null;
    }

    #region Touches
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ITriggerListener triggerListener)) triggerListener.OnTriggerEnterListen(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollisionEnterListerner collisionListerner)) collisionListerner.OnCollisionEnterListen(this);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollisionExitListerner collisionListerner)) collisionListerner.OnCollisionExitListen(this);
    }

    public override void OnExitGround(Ground ground)
    {
        base.OnExitGround(ground);
    }
    public override void OnEnterGround(Ground ground)
    {
        base.OnEnterGround(ground);
    }
    public override void OnTouchBlock(Block block)
    {
        base.OnTouchBlock(block);
    }
    public override void OnTouchedFinish(FinishActor finishActor)
    {
        base.OnTouchedFinish(finishActor);
        StartCoroutine(Win());

    }
    public override void OnTouchedDeadGround(DeadGround deadGround)
    {
        base.OnTouchedDeadGround(deadGround);
        Dead(DeadType.Fall);
       
    }
    #endregion

}

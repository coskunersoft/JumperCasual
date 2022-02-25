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
    [SerializeField][ReadOnly]private bool canFallDown = false;
    [SerializeField] private PhysicMaterial bouncyMaterial;
    [SerializeField] private PhysicMaterial nonBouncyMaterial;
    [SerializeField] private Collider mainCollider;
    private bool falling = false;

    public override void ActorAwake()
    {
        base.ActorAwake();
        Instance = this;
        inputManager = GetComponent<InputManager>();
        mainCollider = GetComponent<Collider>();
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

  

    private void TabController()
    {
        if (canFallDown&&!isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canFallDown = false;
                FallDown();
                return;
            }

        }
       
        if (Input.GetMouseButton(0))
        {
            strecing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (strectAmount > 2.5f)
            {
                canFallDown = true;
                GameManager.Instance.PushEvent(3000);
            }
            strecing = false;
            mainCollider.material = bouncyMaterial;
            if(isGrounded)
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

    private void FallDown()
    {
        falling = true;
        float ro = 0;
        SkinnedMeshRenderer.SetBlendShapeWeight(1,0);
        SkinnedMeshRenderer.SetBlendShapeWeight(2, 0);
        DOTween.To(() => ro, x => ro = x, 100, 0.1f).SetEase(Ease.Linear).OnUpdate(() =>
         {
             SkinnedMeshRenderer.SetBlendShapeWeight(0, ro);
         });
        LinearSpeedReflesh(1, 5);
        mainCollider.material = nonBouncyMaterial;
        Debug.Log("Falled Down");
        if (rotationTween.IsActive()) rotationTween.Kill();
        rb.AddForce((Vector3.down*1.5f+Vector3.forward) * 300);
    }

    protected override void Dead(DeadType deadType)
    {
        base.Dead(deadType);
    }

    protected override IEnumerator WinSquence()
    {
        yield return base.WinSquence();
     
        GameManager.Instance.FinishLevel(!BotPlayer.isFinished);
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
        if (falling)
        {
            falling = false;
            float ro = SkinnedMeshRenderer.GetBlendShapeWeight(0);
            Tween t = null;
            t=DOTween.To(() => ro, x => ro = x, 0, 0.1f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                SkinnedMeshRenderer.SetBlendShapeWeight(0, ro);
                if (strecing&&t!=null) t.Kill();
            });
        }
        
    }
    public override void OnTouchBlock(Block block)
    {
        base.OnTouchBlock(block);
    }
   
    public override void OnTouchedDeadGround(DeadGround deadGround)
    {
        base.OnTouchedDeadGround(deadGround);
       
    }
    #endregion

}

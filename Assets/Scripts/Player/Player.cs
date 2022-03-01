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
    [SerializeField] private PhysicMaterial bouncyMaterial;
    [SerializeField] private PhysicMaterial nonBouncyMaterial;
    [SerializeField] private Collider mainCollider;
    private int FeverPrizeCounter;
    private bool FeverMode;
    [SerializeField] private GameObject FeverEffect;

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
        if (movementLocked) return;
        if (!isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                float ro = SkinnedMeshRenderer.GetBlendShapeWeight(0);
                DOTween.To(() => ro, x => ro = x, 50, 0.2f).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    SkinnedMeshRenderer.SetBlendShapeWeight(0, ro);
                });
                strectAmount = 2.5f;
                Controlledfalling = true;
                if (rotationTween.IsActive()) rotationTween.Kill();

            }
            else if (Input.GetMouseButtonUp(0))
            {
                rb.velocity /= 10;
                Controlledfalling = false;
                float ro = SkinnedMeshRenderer.GetBlendShapeWeight(0);
                DOTween.To(() => ro, x => ro = x, 0, 0.2f).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    SkinnedMeshRenderer.SetBlendShapeWeight(0, ro);
                });
                strectAmount = 0;
            }
            return;
        }

        Controlledfalling = false;
        if (Input.GetMouseButton(0))
        {
            strecing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float externalForce = 1;
            if (strectAmount > 2.5f)
            {
                if (!FeverMode)
                {
                    FeverPrizeCounter++;
                    if (FeverPrizeCounter >= 3)
                    {
                        FeverPrizeCounter = 0;
                        StartCoroutine(FeverModeTimer());
                    }
                }

                if (FeverMode)
                {
                    GameManager.Instance.PushEvent(3001);
                    externalForce = 1.5f;
                    if(FeverEffect)
                    FeverEffect.SetActive(true);
                }
                else
                {
                    GameManager.Instance.PushEvent(3000);
                }
            }
            
            strecing = false;
            mainCollider.material = bouncyMaterial;

          
            Jump(externalForce);
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
        strecing = false;
        base.Dead(deadType);
    }

    protected override IEnumerator WinSquence(FinishActor finishActor)
    {
        yield return base.WinSquence();
        if (!BotPlayer.isFinished)
        {
            finishActor.effect.SetActive(true);
        }
        GameManager.Instance.FinishLevel(!BotPlayer.isFinished);
    }


   private IEnumerator FeverModeTimer()
    {
        movementSpeed*=1.2f;
        FeverMode = true;
        yield return new WaitForSeconds(5f);
        FeverMode = false;
        movementSpeed /= 1.2f;
      
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
        if(FeverEffect)
        if (FeverEffect.activeInHierarchy)
            FeverEffect.SetActive(false);

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

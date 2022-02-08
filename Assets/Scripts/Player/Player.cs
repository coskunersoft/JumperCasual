using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Coskunerov.Tools;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Player : GameSingleActor<Player>
{
    private Rigidbody rb;
    private InputManager inputManager;
    [SerializeField]private float movementSpeed;
    [SerializeField] private float strectSpeed = 1f;
    [SerializeField] private float JumpMultipler = 400;
    [ReadOnly] [SerializeField] private bool isGrounded = false;
    
    private bool strecing = false;
    [ReadOnly][SerializeField] private float strectAmount;
    

    [SerializeField] private SkinnedMeshRenderer SkinnedMeshRenderer;
    private Tween tween;


    public override void ActorAwake()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    public override void ActorUpdate()
    {
        MovementController();
        TabController();
    }

    private void MovementController()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, movementSpeed);
    }

    private void TabController()
    {
        if (!isGrounded) return;

        if (Input.GetMouseButton(0))
        {
            strecing = true;
            tween.Kill();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            strecing = false;
            Jump();
        }

        if (strecing)
        {
            strectAmount += Time.deltaTime*strectSpeed;
            strectAmount = Mathf.Clamp(strectAmount,0, 5f);
            SkinnedMeshRenderer.SetBlendShapeWeight(1, 0);
            SkinnedMeshRenderer.SetBlendShapeWeight(0, (strectAmount / 5f) * 100);
        }
    }

   

    private void Jump()
    {
        tween.Kill();

        float value = SkinnedMeshRenderer.GetBlendShapeWeight(0);
        DOTween.To(() => value, x => value = x, 0, 0.1f).SetEase(Ease.InOutBounce).OnUpdate(()=>
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, value);
        });
       
        rb.AddForce((Vector3.forward + Vector3.up*0.75f) * strectAmount * JumpMultipler);
        strectAmount = 0;
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


    public void OnExitGround(Ground ground)
    {
        isGrounded = false;
    }
    public void OnEnterGround(Ground ground)
    {
        isGrounded = true;

        float hitForce = Mathf.Clamp(Mathf.Abs(rb.velocity.magnitude),0,10);
        if (hitForce > 2)
        if (!tween.IsActive())
            GroundHitAnimation((hitForce/10f)*100);
    }

    private void GroundHitAnimation(float maxValue=100)
    {
        tween.Kill();
        float value = SkinnedMeshRenderer.GetBlendShapeWeight(0);
        float max= maxValue;
        float timex = 0.2f;
        Sequence x = DOTween.Sequence();
        x.Append(DOTween.To(() => value, x => value = x,max, timex)).
            Append(DOTween.To(() => value, x => value = x, 0, timex).OnComplete(() =>
            {
                Debug.Log("xxx");
                timex += 0.05f;
                max -= Random.Range((maxValue*30)/100, (maxValue * 40) / 100);
                if (max <= 0)
                    tween.Kill();
            }))
            .SetLoops(-1,LoopType.Restart).OnUpdate(Updated);
        tween = x;

        void Updated()
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, value);
            SkinnedMeshRenderer.SetBlendShapeWeight(1, value/2);
        }
    }

    #endregion

}

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

    private float horizontalBlendShapeValue;
    private float vibrationTime = 0;
    private float vibrationForce = 0;

    public override void ActorAwake()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    public override void ActorUpdate()
    {
        MovementController();
        TabController();
        VibrationController();
    }

    private void MovementController()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, movementSpeed);
    }

    private void VibrationController()
    {
        vibrationTime += Time.deltaTime;
        if (vibrationTime >= Mathf.PI * 2) vibrationTime = 0;
        horizontalBlendShapeValue = Mathf.Sin(vibrationTime * 10) * 50 * vibrationForce;
        SkinnedMeshRenderer.SetBlendShapeWeight(horizontalBlendShapeValue > 0 ? 1 : 2, horizontalBlendShapeValue > 0 ? horizontalBlendShapeValue : Mathf.Abs(horizontalBlendShapeValue));
    }
    Tween forceMinimizer;
    private void VibrationForceAdd(float force)
    {
        if (force < 3f) return;
        DOTween.To(() => vibrationForce, x => vibrationForce = x, 1, 0.2f).OnComplete(()=>
        {
            if (forceMinimizer.IsActive()) forceMinimizer.Kill();
            forceMinimizer = DOTween.To(() => vibrationForce, x => vibrationForce = x, 0, 1);
        });
        
    }

    private void TabController()
    {
        
        if (!isGrounded) return;

        if (Input.GetMouseButton(0))
        {
            strecing = true;
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
            SkinnedMeshRenderer.SetBlendShapeWeight(0, (strectAmount / 5f) * 100);
        }
       
    }

    private void Jump()
    {
        vibrationForce = 0;

        float value = SkinnedMeshRenderer.GetBlendShapeWeight(0);
        DOTween.To(() => value, x => value = x, 0, 0.1f).SetEase(Ease.InOutBounce).OnUpdate(()=>
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, value);
        });

        float jumpForceFinal = strectAmount * JumpMultipler;
        float jumpForceLimitNormal = jumpForceFinal / (5 * JumpMultipler);
        float value2 = SkinnedMeshRenderer.GetBlendShapeWeight(1);
        DOTween.To(() => value2, x => value2 = x, 65 * jumpForceLimitNormal, 0.2f * jumpForceLimitNormal).SetEase(Ease.InOutBounce).OnUpdate(() =>
            {
                SkinnedMeshRenderer.SetBlendShapeWeight(1, value2);
            });
        rb.AddForce((Vector3.forward + Vector3.up * 0.75f) * jumpForceFinal);
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
        float hitForce = Mathf.Clamp(Mathf.Abs(rb.velocity.y), 0, 4f);
        VibrationForceAdd(hitForce);
    }

    

    #endregion

}

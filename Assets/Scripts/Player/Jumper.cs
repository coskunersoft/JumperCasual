using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Coskunerov.Managers;
using Coskunerov.EventBehaviour;
using Coskunerov.EventBehaviour.Attributes;
using Coskunerov.Tools;
using Sirenix.OdinInspector;
using DG.Tweening;

public abstract class Jumper : GameActor<GameManager>
{
    protected Rigidbody rb;
    protected bool isDead = false;
    [SerializeField] protected ExplodeParts explodeParts;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float rayDistance = 2f;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected bool movementLocked;
    [SerializeField] protected float strectSpeed = 1f;
    [SerializeField] protected float JumpMultipler = 400;
    [ReadOnly] [SerializeField] protected bool isGrounded = false;

    protected bool strecing = false;
    [ReadOnly] [SerializeField] protected float strectAmount;

    [SerializeField] protected SkinnedMeshRenderer SkinnedMeshRenderer;

    protected float horizontalBlendShapeValue;
    protected float vibrationTime = 0;
    protected float vibrationForce = 0;

    protected Vector3 lastCheckPoint;

    public override void ActorStart()
    {
        base.ActorStart();
        LinearSpeedReflesh();
    }

    public override void ActorAwake()
    {
        base.ActorAwake();
        rb = GetComponent<Rigidbody>();
        
    }

    public override void ActorUpdate()
    {
        base.ActorUpdate();
        VibrationController();
    }

    public override void ActorFixedUpdate()
    {
        base.ActorFixedUpdate();
        Ray ray = new Ray(transform.position, -transform.up);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.blue, 0.1f);
        isGrounded = Physics.Raycast(ray, rayDistance, layerMask);
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
        if (force < 1f) return;
        DOTween.To(() => vibrationForce, x => vibrationForce = x, 1, 0.2f).OnComplete(() =>
        {
            if (forceMinimizer.IsActive()) forceMinimizer.Kill();
            forceMinimizer = DOTween.To(() => vibrationForce, x => vibrationForce = x, 0, 1);
        });

    }

    protected Tween rotationTween;
    protected void Jump()
    {
        vibrationForce = 0;
        float value = SkinnedMeshRenderer.GetBlendShapeWeight(0);
        DOTween.To(() => value, x => value = x, 0, 0.1f).SetEase(Ease.InOutBounce).OnUpdate(() =>
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, value);
        });

        float jumpForceFinal = strectAmount * JumpMultipler;
        float jumpForceLimitNormal = jumpForceFinal / (5 * JumpMultipler);
        float value2 = SkinnedMeshRenderer.GetBlendShapeWeight(1);
        DOTween.To(() => value2, x => value2 = x, 65 * jumpForceLimitNormal, 0.2f * jumpForceLimitNormal).SetEase(Ease.InOutBounce).OnUpdate(() =>
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(1, value2);
        }).OnComplete(() =>
        {
            if (jumpForceLimitNormal >= 0.5f)
            {
                int repeatCount = Mathf.Clamp((int)(2f * jumpForceLimitNormal), 0, 2);
                rotationTween = transform.DORotate(transform.eulerAngles + Vector3.right * 360, 0.7f, RotateMode.FastBeyond360).SetLoops(repeatCount, LoopType.Restart).SetEase(Ease.Linear)
                .OnKill(() =>
                {
                    transform.eulerAngles = Vector3.zero;
                });
            }
        });
        rb.AddForce((Vector3.up * 0.8f) * jumpForceFinal*1.5f, ForceMode.Impulse);
        Debug.Log("jUMOP Force :: " + jumpForceFinal);
        strectAmount = 0;
    }

    protected void MovementController()
    {
        if (isGrounded)
            rb.velocity = new Vector3(0, rb.velocity.y, movementSpeed*(movementLocked?0:1));
    }

    protected virtual void Dead(DeadType deadType)
    {
        if (isDead) return;
        isDead = true;
        var respawnTime = 0f;
        switch (deadType)
        {
            case DeadType.SawObstackle:
                GameObject createdexplodeParts = Instantiate(explodeParts.gameObject, explodeParts.transform.position, explodeParts.transform.rotation,explodeParts.transform.parent);
                createdexplodeParts.gameObject.SetActive(true);
                createdexplodeParts.gameObject.transform.SetParent(transform.parent);
                createdexplodeParts.GetComponent<ExplodeParts>().Explode();
                respawnTime = 2;
                break;
            case DeadType.Fall:
                respawnTime = 0.5f;
                break;
        }

        CustomLevelActor.Instance.StartCoroutine(Respawn());
        IEnumerator Respawn()
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(respawnTime);
            if (!GameManager.Instance.runtime.isGameStarted) yield break;
            LinearSpeedReflesh();
            isDead = false;
            rb.velocity = Vector3.zero;
            transform.position = lastCheckPoint;
            gameObject.SetActive(true);
        }
    }

    public void UpdateCheckPoint(Transform checkPoint)
    {
        lastCheckPoint = checkPoint.position;
    }

    Tween speedController;
    protected void LinearSpeedReflesh(float time=0.5f,float divideMultiper=5)
    {
        if (speedController.IsActive()) speedController.Kill();
        float speedTemp = movementSpeed;
        movementSpeed /= 5;
        speedController=DOTween.To(() => movementSpeed, x => movementSpeed = x, speedTemp, time).SetEase(Ease.Linear).OnKill(()=>
        {
            movementSpeed = speedTemp;
        });
    }

    protected virtual IEnumerator WinSquence()
    {
        yield break;
    }

    [GE(BaseGameEvents.FinishGame)]
    public void OnGameFinish()
    {
        rb.velocity = Vector3.zero;
        movementLocked = true;
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

    public virtual void OnExitGround(Ground ground)
    {


    }
    public virtual void OnEnterGround(Ground ground)
    {
        SkinnedMeshRenderer.SetBlendShapeWeight(1, 0);
        float hitForce = Mathf.Clamp(Mathf.Abs(rb.velocity.y), 0, 4f);
        VibrationForceAdd(hitForce);
    }
    public virtual void OnTouchBlock(Block block)
    {
        if(block.blockType==Block.BlockType.Saw)
        Dead(DeadType.SawObstackle);
    }
    public virtual void OnTouchedFinish(FinishActor finishActor)
    {
        StartCoroutine(WinSquence());
    }
    public virtual void OnTouchedDeadGround(DeadGround deadGround)
    {
        Dead(DeadType.Fall);
    }
    #endregion

    public enum DeadType
    {
        SawObstackle,
        Fall,
    }

}

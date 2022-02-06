using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Coskunerov.Tools;
using Sirenix.OdinInspector;

public class Player : GameSingleActor<Player>
{
    private Rigidbody rb;
    private InputManager inputManager;
    [SerializeField]private float movementSpeed;
    [ReadOnly] [SerializeField] private bool isGrounded = false;
    private bool strecing = false;
    [ReadOnly][SerializeField] private float strectAmount;
    public const float JumpMultipler = 400;


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
        }
        else if (Input.GetMouseButtonUp(0))
        {
            strecing = false;
            JumpController();
        }

        if (strecing)
        {
            strectAmount += Time.deltaTime;
        }
    }

    private void JumpController()
    {
        rb.AddForce((Vector3.forward + Vector3.up) * strectAmount * JumpMultipler);
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
    }

    #endregion

}

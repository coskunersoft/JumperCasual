using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Coskunerov.Tools;

public class Player : GameSingleActor<Player>
{
    private Rigidbody rb;
    private InputManager inputManager;
    [SerializeField]private float movementSpeed;


    public override void ActorAwake()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    public override void ActorUpdate()
    {
        MovementController();
    }

    private void MovementController()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, movementSpeed);
    }
}
 
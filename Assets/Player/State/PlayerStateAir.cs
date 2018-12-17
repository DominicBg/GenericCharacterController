using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateAir : PlayerState
{
    [HideInInspector] public UnityEvent OnLandingEvent = new UnityEvent();
    [SerializeField] PlayerDataAir data;

    protected override void OnEnd()
    {
    }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
    }

    protected override void OnFixedUpdate()
    {
        CheckIfLanding();
        Move();
        RotateTowardDirection(data.rotationSpeed);

        if(!Input.GetKey(KeyCode.Space))
        {
            playerRef.physicBody.ResetGravity();
        }
    }

    protected void Move()
    {
        Vector3 direction = playerRef.input.Direction;
        //no input
        if (direction.magnitude == 0)
            return;

        //Going too fast
        float maxDotAllowed = 0.35f;
        Vector3 velocity = playerRef.rigidBody.velocity;
        if (Vector3.Dot(direction, velocity) > maxDotAllowed && playerRef.rigidBody.velocity.magnitude > data.maxSpeed)
            return;

        playerRef.rigidBody.AddForce(direction * data.acceleration, ForceMode.Force);
    }

    protected void CheckIfLanding()
    {
        if (OnGround())
        {
            //Pourrait être landing state
            OnLandingEvent.Invoke();
            playerRef.stateMachine.SetState(State.Idle);
        }
    }
}

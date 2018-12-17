using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerStateGround : PlayerState {

    [HideInInspector] public UnityEvent OnJumpEvent = new UnityEvent();

    protected void Jump(float forwardVelocity, float upwardVelocity)
    {
        Vector3 velocity = playerRef.input.Direction * forwardVelocity + Vector3.up * upwardVelocity;
        playerRef.physicBody.Jump(velocity);
    }

    protected void CheckIfFalling()
    {
        if(!OnGround())
        {
            playerRef.stateMachine.SetState(State.Air);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateWalk : PlayerStateGround
{
    [SerializeField] PlayerDataWalk data;

    protected override void OnEnd()
    {
    }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
        CheckIfFalling();
        Vector3 direction = playerRef.input.Direction;

        if (direction.magnitude == 0)
            playerRef.stateMachine.SetState(State.Idle);

        MoveRotate(direction, data.walkSpeed, data.rotationSpeed);

        //playerRef.transform.position += direction * data.walkSpeed * Time.deltaTime;
        //RotateTowardDirection(data.rotationSpeed);

        //OnWalkEvent.Invoke(direction.magnitude);

        // if(player.input.getButtonDown) mettre rewired
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRef.physicsBody.ResetGravity(data.physicsResetMin, data.physicsResetTime);
            Jump(data.jumpForwardVelocity, data.jumpUpwardVelocity);
        }
    }
}

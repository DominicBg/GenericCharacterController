using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerStateGround : PlayerState {

    [HideInInspector] public UnityEvent OnJumpEvent = new UnityEvent();
    [HideInInspector] public UnityFloatEvent OnWalkEvent = new UnityFloatEvent();

    protected void Jump(float forwardVelocity, float upwardVelocity)
    {
        Vector3 velocity = playerRef.input.Direction * forwardVelocity + Vector3.up * upwardVelocity;
        playerRef.physicsBody.Jump(velocity);
    }

    protected void CheckIfFalling()
    {
        if(!OnGround())
        {
            SetState(PlayerStateEnumConst.State.PlayerStateAir);
        }
    }
    protected void Move(Vector3 direction, float moveSpeed)
    {
        if (direction.magnitude < 0.01f)
            return;

        Vector3 destination = GetAjustedDestination(direction);
        Vector3 trueDirection = (destination - playerRef.transform.position);

        if (!HasWallCollision(trueDirection))
        {
            //Vector3 destination = GetAjustedDestination(direction);
            //playerRef.transform.position += direction * moveSpeed * Time.deltaTime;
            playerRef.transform.position += trueDirection * moveSpeed * Time.deltaTime;
            //playerRef.transform.position += trueDirection * moveSpeed * Time.fixedDeltaTime;

        }
        else
        {
            float angle = 55;

            bool tryRightAngle = TryDirection(angle, moveSpeed);
            if (!tryRightAngle)
                TryDirection(-angle, moveSpeed);
        }
        OnWalkEvent.Invoke(direction.magnitude);
    }

    bool TryDirection(float angle, float moveSpeed)
    {
        Vector3 direction = GameMath.RotateVectorY(angle, playerRef.transform.forward);
        if (!playerRef.colliderDetection.GridRayCast(direction).hasHit)
        {
            playerRef.transform.position += direction * moveSpeed * Time.deltaTime;
            return true;
        }
        return false;
    }

    protected void MoveRotate(Vector3 direction, float moveSpeed, float rotationSpeed)
    {
        Move(direction, moveSpeed);
        RotateTowardDirection(rotationSpeed);
    }

}

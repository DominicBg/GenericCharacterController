using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerStateGround
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRef.physicsBody.ResetGravity(data.physicsResetMin, data.physicsResetTime);
            Jump(0, data.jumpUpwardVelocity);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetState(PlayerStateEnumConst.State.PlayerStateAttack);
        }

        if (playerRef.input.Direction.magnitude != 0)
            playerRef.stateMachine.SetState(PlayerStateEnumConst.State.PlayerStateWalk);
    }
}

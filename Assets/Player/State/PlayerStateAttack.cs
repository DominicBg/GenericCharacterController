using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack : PlayerState
{
    [SerializeField] float dashDuration;
    [SerializeField] float dashDistance;

    protected override void OnEnd()
    {
    }

    protected override void OnStart()
    {
        Debug.Log("I attack");
        Translate(playerRef.transform.forward * dashDistance, dashDuration, ExitState);
    }

    protected override void OnUpdate()
    {
        if(timeInState > dashDuration * 1.1f)
        {
            ExitState();
        }
    }
}

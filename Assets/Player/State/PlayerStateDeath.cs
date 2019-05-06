using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDeath : PlayerState
{
    protected override void OnEnd()
    {
    }

    protected override void OnStart()
    {
        Debug.Log("I die");
        ExitState();
    }
}

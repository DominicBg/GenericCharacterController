using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerStateKnocked : PlayerStateGround
{
    DamageInfo damageInfo;
    Vector3 destination;
    float duration;

    private void Start()
    {
        IsInvincible = true;
    }

    public void SetKnocked(DamageInfo damageInfo, Vector3 force)
    {
        this.damageInfo = damageInfo;
        duration = damageInfo.data.knockbackDuration;
        Translate(force, damageInfo.data.knockbackDuration, ExitState, null, damageInfo.data.knockbackCurve);
    }

    protected override void OnEnd()
    {
        StopTranslate();
        //SetInvincible(playerData.invincibilityAfterKnockback);
    }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
        //Safety check if translate bug
        if(timeInState > duration * 1.1f)
        {
            ExitState();
        }
    }
}

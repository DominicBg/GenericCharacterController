using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public abstract class PlayerState : CharacterState
{
    //public enum State { Idle, Walk, Attacking, Knocked, Air, Death };
    //public abstract State state { get; }
    //public State state;

    protected PlayerStateMachine.PlayerRef playerRef;
    protected PlayerData playerData;

    protected bool CanInteractWithString = false;
    protected bool IsInvincible = false;

    bool hasChangedState = false;

    protected sealed override void OnPreStart()
    {
        if (IsInvincible)
            playerRef.healthComponent.TryStopInvincibleCoroutine();

        //Si y'est invincible a cause d'un hit, laisse le timer finir
        if (!playerRef.healthComponent.IsTemporaryInvincible)
            playerRef.healthComponent.IsInvincible = IsInvincible;
    }

    protected sealed override void OnPreUpdate()
    {
        if (IsInvincible)
            playerRef.healthComponent.IsInvincible = IsInvincible;
    }

    protected sealed override void OnLateUpdate()
    {
        hasChangedState = false;
    }

    public void Initialize(PlayerStateMachine.PlayerRef playerRef, PlayerData playerData)
    {
        this.playerRef = playerRef;
        this.playerData = playerData;
    }


    protected void ExitState()
    {
        if (hasChangedState)
            return;

        if (OnGround())
            SetState(PlayerStateEnumConst.State.PlayerStateIdle);
        else
            SetState(PlayerStateEnumConst.State.PlayerStateAir);
    }

    bool StillInCurrentState()
    {
        return GetType() == playerRef.stateMachine.GetCurrentState().GetType();
    }

    protected bool IsStateOnCooldown(PlayerStateEnumConst.State state)
    {
        return playerRef.stateMachine.IsStateOnCooldown(state);
    }

    protected void SetState(PlayerStateEnumConst.State state)
    {
        if (hasChangedState)
            return;

        hasChangedState = true;
        playerRef.stateMachine.SetState(state);
    }

    protected void RotateTowardDirection(float speed = 100)
    {
        playerRef.stateMachine.RotateTowardDirection(playerRef.input.Direction, speed);
    }

    protected void RotateTowardDirection(Vector2 joystickDirection, float speed = 100)
    {
        playerRef.stateMachine.RotateTowardDirection(playerRef.input.GetDirection(joystickDirection), speed);
    }

    /// <summary>
    /// Translate with a tween, at the end of tween, onFinishCallback will be invoked, if there's a collisions in the way, onCollisionCallback is invoked with the remaining tmie
    /// </summary>
    protected void Translate(Vector3 position, Vector3 destination, float duration, System.Action onFinishCallback = null, System.Action<float> onCollisionCallback = null, Ease ease = Ease.InOutSine)
    {
        Vector3 direction = destination - position;
        Translate(direction, duration, onFinishCallback, onCollisionCallback, ease);
    }


    /// <summary>
    /// Translate with a tween, at the end of tween, onFinishCallback will be invoked, if there's a collisions in the way, onCollisionCallback is invoked with the remaining tmie
    /// </summary>
    protected void Translate(Vector3 direction, float duration, System.Action onFinishCallback = null, System.Action<float> onCollisionCallback = null, Ease ease = Ease.InOutSine)
    {
        Vector3 destination = playerRef.colliderDetection.GetAjustedDestination(
            playerRef.transform.position,
            direction).point;

        playerRef.transform.DOKill();
        if (ContinuousTranslateDetectionCoroutine != null)
            StopCoroutine(ContinuousTranslateDetectionCoroutine);

        //if(onFinishCallback == null)
        //    playerRef.transform.DOMove(destination, duration).SetEase(ease);
        //else
        playerRef.transform.DOMove(destination, duration).SetEase(ease);

        ContinuousTranslateDetectionCoroutine = StartCoroutine(ContinuousTranslateDetection(direction, duration, onFinishCallback, onCollisionCallback));
    }

    protected void StopTranslate()
    {
        playerRef.transform.DOKill();
        if (ContinuousTranslateDetectionCoroutine != null)
            StopCoroutine(ContinuousTranslateDetectionCoroutine);
    }

    Coroutine ContinuousTranslateDetectionCoroutine;
    IEnumerator ContinuousTranslateDetection(Vector3 direction, float duration, System.Action onFinishCallback = null, System.Action<float> onCollisionCallback = null)
    {
        float t = 0;
        bool hasCollision = false;

        while (t < duration && !hasCollision)
        {
            t += Time.deltaTime;
            if (playerRef.colliderDetection.GridRayCast(direction).hasHit)
            {
                playerRef.transform.DOKill();

                if (onCollisionCallback != null)
                {
                    //temps restant
                    onCollisionCallback.Invoke(duration - t);
                    hasCollision = true;
                }
                t = duration;
            }
            yield return null;
        }

        if (!hasCollision && onFinishCallback != null)
        {
            onFinishCallback.Invoke();
        }
    }

    protected ColliderDetection.PointDetectionInfo GetAjustedDestinationInfo(Vector3 direction)
    {
        return playerRef.colliderDetection.GetAjustedDestination(playerRef.transform.position, direction);
    }

    protected Vector3 GetAjustedDestination(Vector3 direction)
    {
        return playerRef.colliderDetection.GetAjustedDestination(playerRef.transform.position, direction).point;
    }

    protected bool HasWallCollision(Vector3 direction)
    {
        return playerRef.colliderDetection.GridRayCast(direction).hasHit;
    }
    protected bool HasWallCollision()
    {
        return playerRef.colliderDetection.collisionData.hasHit;
    }

    //protected PlayerInput Input
    //{
    //    get
    //    {
    //        return playerRef.input;
    //    }
    //}

    protected bool OnGround()
    {
        return playerRef.colliderDetection.collisionData.onGround;
    }


    protected bool WasLastStateOnGround()
    {
        return playerRef.stateMachine.GetLastState().GetType().IsSubclassOf(typeof(PlayerStateGround));
    }

    public bool IsStateAvailable(PlayerStateEnumConst.State state)
    {
        return playerRef.stateMachine.GetState(state).IsAvailable();

    }

    protected void SetInvincible()
    {
        playerRef.healthComponent.IsInvincible = true;
    }

    protected void SetInvincible(float duration, System.Action callback = null)
    {
        playerRef.healthComponent.SetInvincible(duration, callback);
    }

    protected virtual bool IsAvailable()
    {
        return !IsOnCooldown;
    }

    public bool IsStateOnGround()
    {
        return GetType().IsSubclassOf(typeof(PlayerStateGround));
    }


    public virtual void OnAnimationEvent(string eventName)
    {
        //override me
    }
}

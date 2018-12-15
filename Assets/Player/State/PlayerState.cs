using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerState : MonoBehaviour
{
    public enum State { Idle, Walk, Running, Air };

    [HideInInspector] public UnityEvent OnStartEvent = new UnityEvent();
    [HideInInspector] public UnityEvent OnEndEvent = new UnityEvent();

    protected float timeInState;
    protected PlayerStateMachine.Player playerRef;
    protected PlayerData playerData;

    #region StateMachine
    public void Initialize(PlayerStateMachine.Player playerRef, PlayerData playerData)
    {
        this.playerRef = playerRef;
        this.playerData = playerData;
    }

    public void StartState()
    {
        timeInState = 0;
        OnStartEvent.Invoke();
        OnStart();
    }
    public void UpdateState()
    {
        timeInState += Time.deltaTime;
        OnUpdate();
    }
    public void FixedUpdateState()
    {
        OnFixedUpdate();
    }

    public void EndState()
    {
        OnEndEvent.Invoke();
        OnEnd();
    }

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected virtual void OnFixedUpdate() { }
    protected abstract void OnEnd();
    #endregion

    protected bool OnGround()
    {
        //Raycast, mettre layer mask maybe
        float maxDistance = .5f;
        Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left };
        int count = 0;
        foreach(Vector3 dir in directions)
        {
            Vector3 startPosition = playerRef.lowestPointTransform.position + dir * playerData.groundDetectionSize;
            Debug.DrawRay(startPosition, Vector3.down * maxDistance);
            if (Physics.Raycast(startPosition, Vector3.down, maxDistance))
                count++;
        }

        if (count > 0)
            return true;

        return false;
    }

    protected void RotateTowardDirection(float speed)
    {
        Vector3 direction = playerRef.input.Direction;
        if (direction.magnitude == 0)
            return;

        float scaleUpFactor = 100;
        Quaternion from = playerRef.transform.rotation;
        Quaternion to = Quaternion.LookRotation(direction, playerRef.transform.up);
        playerRef.transform.rotation = Quaternion.RotateTowards(from, to, speed * Time.deltaTime * scaleUpFactor);
    }


    public virtual void OnButtonPressed() { }

    public class UnityFloatEvent : UnityEvent<float> { }
    public class UnityIntEvent : UnityEvent<int> { }
    public class UnityBoolEvent : UnityEvent<bool> { }
}

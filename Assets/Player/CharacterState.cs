using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterState : MonoBehaviour {

    public bool IsOnCooldown { get; private set; }

    protected float timeInState;

    public UnityEvent OnStartEvent = new UnityEvent();
    public UnityEvent OnEndEvent = new UnityEvent();

    public float cooldown { get; private set; }
    private System.Action cooldownCallback;

    public void StartState()
    {
        timeInState = 0;
        OnPreStart();
        OnStart();
        OnStartEvent.Invoke();
    }
    public void UpdateState()
    {
        timeInState += Time.deltaTime;
        OnPreUpdate();
        OnUpdate();
        OnLateUpdate();
    }
    public void FixedUpdateState()
    {
        OnFixedUpdate();
    }

    public void EndState()
    {
        OnEndEvent.Invoke();
        OnEnd();
        OnLateEnd();
    }

    protected abstract void OnPreStart();

    protected abstract void OnStart();
    protected abstract void OnEnd();
    protected virtual void OnLateEnd() { }

    protected virtual void OnUpdate() { }
    protected virtual void OnPreUpdate() { }
    protected virtual void OnLateUpdate() { }

    protected virtual void OnFixedUpdate() { }

    protected void StartCooldown(float cooldown, System.Action callback = null)
    {
        this.cooldown = cooldown;
        IsOnCooldown = true;
        cooldownCallback = callback;
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void UpdateCooldown()
    {
        if (IsOnCooldown)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                if (cooldownCallback != null)
                {
                    cooldownCallback.Invoke();
                }

                IsOnCooldown = false;
            }
        }
    }

    //Events
    public class UnityFloatEvent : UnityEvent<float> { }
    public class UnityIntEvent : UnityEvent<int> { }
    public class UnityBoolEvent : UnityEvent<bool> { }
}

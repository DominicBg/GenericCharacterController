using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour {

    [SerializeField] protected Animator animator;
    [SerializeField] PlayerStateMachine stateMachine;
    void Start()
    {
        InitializeWalk();
        InitializeIdle();
        InitializeAir();
    }

    void InitializeWalk()
    {
        PlayerStateWalk walkState = (PlayerStateWalk)stateMachine.GetState(PlayerState.State.Walk);
        walkState.OnWalkEvent.AddListener((speed) => animator.SetFloat(PlayerAnimationConst.MoveSpeed, speed));
        walkState.OnStartEvent.AddListener(() => animator.SetBool(PlayerAnimationConst.Grounded, true));
        walkState.OnJumpEvent.AddListener(() => animator.SetTrigger(PlayerAnimationConst.Jump));
    }

    void InitializeIdle()
    {
        PlayerStateIdle walkState = (PlayerStateIdle)stateMachine.GetState(PlayerState.State.Idle);

        walkState.OnStartEvent.AddListener(() => animator.SetFloat(PlayerAnimationConst.MoveSpeed, 0f));
        walkState.OnStartEvent.AddListener(() => animator.SetBool(PlayerAnimationConst.Grounded, true));
        walkState.OnJumpEvent.AddListener(() => animator.SetTrigger(PlayerAnimationConst.Jump));
    }

    void InitializeAir()
    {
        PlayerStateAir airState = (PlayerStateAir)stateMachine.GetState(PlayerState.State.Air);
        airState.OnStartEvent.AddListener(() => animator.SetBool(PlayerAnimationConst.Grounded, false));
    }
}

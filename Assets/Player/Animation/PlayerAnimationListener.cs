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
        PlayerStateWalk walkState = (PlayerStateWalk)stateMachine.GetState(PlayerStateEnumConst.State.PlayerStateWalk);
        walkState.OnWalkEvent.AddListener((speed) => animator.SetFloat(PlayerAnimationConst.MoveSpeed, speed));
        walkState.OnStartEvent.AddListener(() => animator.SetBool(PlayerAnimationConst.Grounded, true));
        walkState.OnJumpEvent.AddListener(() => animator.SetTrigger(PlayerAnimationConst.Jump));
    }

    void InitializeIdle()
    {
        PlayerStateIdle walkState = (PlayerStateIdle)stateMachine.GetState(PlayerStateEnumConst.State.PlayerStateIdle);

        walkState.OnStartEvent.AddListener(() => animator.SetFloat(PlayerAnimationConst.MoveSpeed, 0f));
        walkState.OnStartEvent.AddListener(() => animator.SetBool(PlayerAnimationConst.Grounded, true));
        walkState.OnJumpEvent.AddListener(() => animator.SetTrigger(PlayerAnimationConst.Jump));
    }

    void InitializeAir()
    {
        PlayerStateAir airState = (PlayerStateAir)stateMachine.GetState(PlayerStateEnumConst.State.PlayerStateAir);
        airState.OnStartEvent.AddListener(() => animator.SetBool(PlayerAnimationConst.Grounded, false));
    }
}

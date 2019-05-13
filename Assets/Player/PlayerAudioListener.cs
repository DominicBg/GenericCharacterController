using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioListener : MonoBehaviour {

    [SerializeField] PlayerStateMachine stateMachine;
	void Start ()
    {
        InitializeWalk();
    }

    void InitializeWalk()
    {
        PlayerStateWalk walkState = (PlayerStateWalk)stateMachine.GetState(PlayerStateEnumConst.State.PlayerStateWalk);
        walkState.OnStartEvent.AddListener(() => PlaySound("Start walk what ever"));
        walkState.OnJumpEvent.AddListener(() => PlaySound("Jump"));
    }

    public void PlaySound(string soundName)
    {
        //Sound logic
    }
}

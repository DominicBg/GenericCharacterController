using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour {

    [SerializeField] PlayerState.State currentStateEnum;

    [SerializeField] Player playerRef;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform stateTransform;

    PlayerState currentState;
    Dictionary<PlayerState.State, PlayerState> stateDictionary = new Dictionary<PlayerState.State, PlayerState>();

    private void Awake()
    {
        InitialiseStateDictionary();
        SetState(PlayerState.State.Idle);
    }

    void InitialiseStateDictionary()
    {
        InitialiseState(PlayerState.State.Idle, stateTransform.GetComponent<PlayerStateIdle>());
        InitialiseState(PlayerState.State.Walk, stateTransform.GetComponent<PlayerStateWalk>());
        InitialiseState(PlayerState.State.Air, stateTransform.GetComponent<PlayerStateAir>());
    }

    void InitialiseState(PlayerState.State stateEnum, PlayerState state)
    {
        state.Initialize(playerRef, playerData);
        stateDictionary.Add(stateEnum, state);
    }

    private void Update()
    {
        currentState.UpdateState();
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void SetState(PlayerState.State state)
    {
        if (currentState != null)
            currentState.EndState();

        currentState = stateDictionary[state];
        currentState.StartState();
        currentStateEnum = state;
    }

    public PlayerState GetState(PlayerState.State state)
    {
        return stateDictionary[state];
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }

    [System.Serializable]
    public class Player
    {
        [Header("References")]
        public PlayerStateMachine stateMachine;
        public PlayerInput input;
        public Transform transform;
        public Transform lowestPointTransform;
        public Rigidbody rigidBody;
        public PhysicsBody physicBody;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : CharacterStateMachine
{

    [SerializeField] PlayerState.State currentStateEnum;

    [SerializeField] PlayerRef playerRef;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform stateTransform;

    PlayerState currentState;
    PlayerState lastState;
    PlayerState.State lastStateEnum;

    Dictionary<PlayerState.State, PlayerState> stateDictionary = new Dictionary<PlayerState.State, PlayerState>();

    private void Awake()
    {
        InitialiseStateDictionary();
        SetState(PlayerState.State.Idle);
        base.Awake();
    }

    void Start()
    {
        base.Start();
        playerRef.healthComponent.OnDeathEvent.AddListener((damageInfo) => SetState(PlayerState.State.Death));
    }

    void InitialiseStateDictionary()
    {
        InitialiseState(PlayerState.State.Idle, stateTransform.GetComponent<PlayerStateIdle>());
        InitialiseState(PlayerState.State.Walk, stateTransform.GetComponent<PlayerStateWalk>());
        InitialiseState(PlayerState.State.Attacking, stateTransform.GetComponent<PlayerStateAttack>());
        InitialiseState(PlayerState.State.Knocked, stateTransform.GetComponent<PlayerStateKnocked>());
        InitialiseState(PlayerState.State.Air, stateTransform.GetComponent<PlayerStateAir>());
        InitialiseState(PlayerState.State.Death, stateTransform.GetComponent<PlayerStateDeath>());

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

    public bool IsStateOnCooldown(PlayerState.State state)
    {
        return stateDictionary[state].IsOnCooldown;
    }

    public void SetState(PlayerState.State state)
    {
        if (currentState != null)
        {
            currentState.EndState();
            lastStateEnum = currentStateEnum;
            lastState = currentState;
        }
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

    public PlayerState GetLastState()
    {
        return lastState;
    }
    public PlayerState.State GetLastStateEnum()
    {
        return lastStateEnum;
    }

    public override void Knockback(DamageInfo damageInfo, Vector3 force)
    {
        PlayerStateKnocked state = (PlayerStateKnocked)stateDictionary[PlayerState.State.Knocked];
        state.SetKnocked(damageInfo, force);
        

        SetState(PlayerState.State.Knocked);
    }

    public void OnAnimationEvent(string eventName)
    {
        currentState.OnAnimationEvent(eventName);
    }

    public void RotateTowardDirection(Vector3 direction, float speed = 100)
    {
        //Vector3 direction = playerRef.input.Direction;
        if (direction.magnitude == 0)
            return;

        float scaleUpFactor = 100;
        Quaternion from = playerRef.transform.rotation;
        Quaternion to = Quaternion.LookRotation(direction, playerRef.transform.up);
        playerRef.transform.rotation = Quaternion.RotateTowards(from, to, speed * Time.deltaTime * scaleUpFactor);
    }

    public PlayerRef GetPlayerRef()
    {
        return playerRef;
    }

    [System.Serializable]
    public class PlayerRef
    {
        [Header("References")]
        public PlayerStateMachine stateMachine;
        public PlayerInput input;
        public Transform transform;
        public Transform avatar;
        public Transform lowestPointTransform;
        public Rigidbody rigidBody;
        public HealthComponent healthComponent;
        public PhysicsBody physicsBody;
        public ColliderDetection colliderDetection;
        public CapsuleCollider capsuleCollider;
    }
}

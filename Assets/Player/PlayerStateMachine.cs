using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStateMachine : CharacterStateMachine
{
    [SerializeField] PlayerStateEnumConst.State currentStateEnum;

    [SerializeField] PlayerRef playerRef;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform stateTransform;

    PlayerState currentState;
    PlayerState lastState;
    PlayerStateEnumConst.State lastStateEnum;

    Dictionary<PlayerStateEnumConst.State, PlayerState> stateDictionary = new Dictionary<PlayerStateEnumConst.State, PlayerState>();

    private void Awake()
    {
        InitialiseStateDictionary();
        SetState(PlayerStateEnumConst.State.PlayerStateIdle);
        base.Awake();
    }

    void Start()
    {
        base.Start();
        playerRef.healthComponent.OnDeathEvent.AddListener((damageInfo) => SetState(PlayerStateEnumConst.State.PlayerStateDeath));
    }

    [ContextMenu("Test")]
    void Test()
    {
        List<string> names = System.AppDomain.CurrentDomain.GetAssemblies()
             .SelectMany(x => x.GetTypes())
             .Where(x => typeof(PlayerState).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
             .Select(x => x.Name).ToList();

        ConstFileWriter.GenerateConstFile(this, "PlayerStateConst", names.ToArray());
        ConstFileWriter.GenerateEnumConstFile(this, "PlayerStateEnumConst", "State", names.ToArray());
    }

    void InitialiseStateDictionary()
    {
        PlayerState[] playerStates = stateTransform.GetComponents<PlayerState>();
        foreach (PlayerState state in playerStates)
        {
            PlayerStateEnumConst.State value = (PlayerStateEnumConst.State)System.Enum.Parse(typeof(PlayerStateEnumConst.State), state.GetType().Name);
            print(value);

            InitialiseState(value, state);
        }
    }

    void InitialiseState(PlayerStateEnumConst.State stateEnum, PlayerState state)
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

    public bool IsStateOnCooldown(PlayerStateEnumConst.State state)
    {
        return stateDictionary[state].IsOnCooldown;
    }

    public void SetState(PlayerStateEnumConst.State state)
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

    public PlayerState GetState(PlayerStateEnumConst.State state)
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
    public PlayerStateEnumConst.State GetLastStateEnum()
    {
        return lastStateEnum;
    }

    public override void Knockback(DamageInfo damageInfo, Vector3 force)
    {
        PlayerStateKnocked state = (PlayerStateKnocked)stateDictionary[PlayerStateEnumConst.State.PlayerStateKnocked];
        state.SetKnocked(damageInfo, force);
        

        SetState(PlayerStateEnumConst.State.PlayerStateKnocked);
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

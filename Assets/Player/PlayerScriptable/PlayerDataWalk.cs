using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkData", menuName = "Player/PlayerData/WalkData", order = 1)]
public class PlayerDataWalk : ScriptableObject {
    public float acceleration;
    public float walkSpeed;
  
    public float rotationSpeed;

    [Header("Physics")]
    public float physicsResetTime;
    public float physicsResetMin;
    public float jumpForwardVelocity;
    public float jumpUpwardVelocity;
}

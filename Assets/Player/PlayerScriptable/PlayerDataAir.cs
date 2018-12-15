using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirData", menuName = "Player/PlayerData/AirData", order = 1)]
public class PlayerDataAir : ScriptableObject {
    public float acceleration;
    public float maxSpeed;
    public float rotationSpeed;
}

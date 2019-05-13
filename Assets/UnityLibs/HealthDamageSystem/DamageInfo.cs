using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class DamageInfo
{
    public Team team;
    public GameObject owner;
    public DamageData data;
}
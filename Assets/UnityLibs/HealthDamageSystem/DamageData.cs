using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageData", menuName = "Data/Others/DamageData", order = 1)]
public class DamageData : ScriptableObject
{
    public WeaponType weaponType;

    [Header("Damage")]
    public int damage;

    [Header("Knockback")]
    public float knockbackDuration;
    public DG.Tweening.Ease knockbackCurve;
    public float knockbackDistance;
    public float knockbackHeight;
}

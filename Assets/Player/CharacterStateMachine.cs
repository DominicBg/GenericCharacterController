using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class CharacterStateMachine : MonoBehaviour
{
    public abstract void Knockback(DamageInfo damageInfo, Vector3 force);

    Rigidbody rigidBody;
    protected HealthComponent healthComponent;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        healthComponent = GetComponent<HealthComponent>();
    }

    protected void Start()
    {
        healthComponent.OnDamageEvent.AddListener((current, max, damageData) => TakeDamage(damageData));
    }

    void TakeDamage(DamageInfo damageInfo)
    {
        Vector3 diff = (transform.position - damageInfo.owner.transform.position);
        diff = diff.SetY(0);
        diff.Normalize();

        Knockback(damageInfo, diff * damageInfo.data.knockbackDistance);
    }
}

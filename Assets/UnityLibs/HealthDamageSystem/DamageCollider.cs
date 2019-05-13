using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageCollider : MonoBehaviour
{
    DamageInfo damageInfo;
    [HideInInspector] public HealthComponentEvent OnDealDamageEvent = new HealthComponentEvent();

    HealthComponent hpComponent;

    public void SetDamageInfo(DamageInfo damageInfo)
    {
        this.damageInfo = damageInfo;
    }

    private void OnTriggerEnter(Collider other)
    {
        hpComponent = other.GetComponent<HealthComponent>();
        if (hpComponent && !hpComponent.IsFriendlyFire(damageInfo))
        {
            hpComponent.TakeDamage(damageInfo);
            OnDealDamageEvent.Invoke(hpComponent);
        }
    }

    public class HealthComponentEvent : UnityEvent<HealthComponent>{};
}

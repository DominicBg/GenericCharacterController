using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum Team { Team1, Team2, All}
public enum WeaponType { Normal, None, All}
public class HealthComponent : MonoBehaviour
{
    public Team currentTeam;
    public bool IsInvincible { get; set; }
    public bool IsDeath { get; private set; }
    public bool IsTemporaryInvincible { get; private set; }

    [SerializeField] int maxHealth = 10;
    [SerializeField] WeaponType affectedBy = WeaponType.All;

    public HealthDamageEvent OnDamageEvent { get; private set; }
    public HealthEvent OnHealEvent { get; private set; }

    public DeathEvent OnDeathEvent { get; private set; }
    public UnityEvent OnNegatedDamageEvent { get; private set; }

    int currentHealth;

    Coroutine invincibleCoroutine;

    void Awake()
    {
        OnDamageEvent = new HealthDamageEvent();
        OnHealEvent = new HealthEvent();
        OnDeathEvent = new DeathEvent();
        OnNegatedDamageEvent = new UnityEvent();
        currentHealth = maxHealth;
    }

    public void SetHealth(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        IsDeath = false;
        OnHealEvent.Invoke(currentHealth, maxHealth);
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsFriendlyFire(DamageInfo damageData)
    {     
        return currentTeam == damageData.team;
    }

    public void TakeDamage(DamageInfo damageData)
    {
        if (IsDeath || affectedBy == WeaponType.None)
            return;

        bool canBeAffected = (affectedBy == WeaponType.All) || (damageData.data.weaponType == affectedBy);
        if (!canBeAffected)
            return;

        if (IsInvincible)
        {
            OnNegatedDamageEvent.Invoke();
            return;
        }

        currentHealth -= damageData.data.damage;

        if(currentHealth <= 0)
        {
            OnDeathEvent.Invoke(damageData);
            IsDeath = true;
        }
        else
        {
            OnDamageEvent.Invoke(currentHealth, maxHealth, damageData);
        }
    }

    public void Heal(int heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealEvent.Invoke(currentHealth,maxHealth);
    }

    public void SetInvincible(float secInvincible, System.Action endInvincibleCallback = null)
    {
        if (invincibleCoroutine != null)
            StopCoroutine(invincibleCoroutine);

        invincibleCoroutine = StartCoroutine(DelayInvincible(secInvincible, endInvincibleCallback));
    }

    public void TryStopInvincibleCoroutine()
    {
        if (invincibleCoroutine != null)
            StopCoroutine(invincibleCoroutine);

        IsInvincible = false;
        IsTemporaryInvincible = false;
    }

    IEnumerator DelayInvincible(float secInvincible, System.Action endInvincibleCallback = null)
    {
        IsTemporaryInvincible = true;
        IsInvincible = true;

        yield return new WaitForSeconds(secInvincible);

        IsInvincible = false;
        IsTemporaryInvincible = false;

        if (endInvincibleCallback != null)
            endInvincibleCallback.Invoke();
    }

    public class DeathEvent : UnityEvent<DamageInfo> { }
    public class HealthDamageEvent : UnityEvent<int, int, DamageInfo>{}
    public class HealthEvent : UnityEvent<int, int> { }

}

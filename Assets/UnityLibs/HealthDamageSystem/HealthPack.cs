using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    [SerializeField] Team usableBy;
    [SerializeField] int healAmmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        HealthComponent hpComponent = other.GetComponent<HealthComponent>();
        if (usableBy == Team.All || hpComponent && hpComponent.currentTeam == usableBy)
        {
            hpComponent.Heal(1);
            OnCollected();
                
        }
    }
    void OnCollected()
    {
        //Particle effects
        //Sound

        Destroy(gameObject);
    }
}

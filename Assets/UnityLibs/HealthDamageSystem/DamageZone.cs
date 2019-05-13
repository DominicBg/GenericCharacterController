using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : DamageCollider {

    [SerializeField] DamageInfo info;

    private void Start()
    {

        SetDamageInfo(info);
    }
}

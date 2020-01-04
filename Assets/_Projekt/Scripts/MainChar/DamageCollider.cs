using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public bool isPlayerWeapon;
    public bool isEnabled = true;

    private void OnTriggerEnter(Collision collision)
    {
        if (!isEnabled) return;

        var recv = collision.gameObject.GetComponent<DamageReciever>();
        if (recv.IsPlayer() != isPlayerWeapon)
        {
            Debug.Log("DamageCollider: do Damage");
            recv.DoDamage();
        }
    }
}

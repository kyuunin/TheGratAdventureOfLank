using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageReciever : MonoBehaviour
{
    public abstract bool IsPlayer();
    public abstract void DoDamage();
}

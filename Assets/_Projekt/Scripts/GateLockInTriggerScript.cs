using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLockInTriggerScript : MonoBehaviour
{
    public GameObject gate;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            gate.SetActive(true);
    }
}

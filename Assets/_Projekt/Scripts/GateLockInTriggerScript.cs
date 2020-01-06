using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLockInTriggerScript : MonoBehaviour
{
    public GameObject gate;
    public bool gateClosed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !gateClosed)
        {
            gateClosed = true;
            gate.SetActive(true);
            GameObject.FindObjectOfType<MusicManager>().PlayBossMusic();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public static int CoinsCollected { get; private set; }
    public static int CoinsSpawned { get; private set; }

    public static int GetRequiredCoinCount()
    {
        float f = CoinsSpawned * 0.80f;
        return (int) Mathf.Ceil(f);
    }

    void Awake()
    {
        CoinsSpawned++;
        Debug.Log("Coin Spawned!");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CoinsCollected++;
            Destroy(gameObject);
        }
    }
}

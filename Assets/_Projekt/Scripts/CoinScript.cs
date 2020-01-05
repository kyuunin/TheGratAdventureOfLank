using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public static int CoinsCollected { get; private set; }
    public static int CoinsSpawned { get; private set; }

    public int value = 1;
    public bool shouldRotate = true;
    public GameObject spawnOnCollect = null;

    public static int GetRequiredCoinCount()
    {
        float f = CoinsSpawned * 0.80f;
        return (int) Mathf.Ceil(f);
    }

    void Awake()
    {
        CoinsSpawned += value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CoinsCollected += value;
            if (spawnOnCollect != null)
                Instantiate(spawnOnCollect, transform.position, Quaternion.Euler(-90,0,0), transform.parent);
            Destroy(gameObject, 0.1f);
        }
    }

    private float angle = 0;
    void Update()
    {
        if(shouldRotate)
        {
            angle += Time.deltaTime * 360f * 0.5f;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}

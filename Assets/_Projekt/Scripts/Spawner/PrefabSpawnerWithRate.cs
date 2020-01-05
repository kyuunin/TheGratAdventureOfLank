using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawnerWithRate : MonoBehaviour
{
    public GameObject prefab;
    public float probability = 0.25f;
    
    void Start()
    {
        if (Random.Range(0f, 1f) < probability)
        {
            Instantiate(prefab, transform.position, transform.rotation, transform.parent);   
        }
        Destroy(gameObject, 1.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColorChanger : MonoBehaviour
{
    public Material damageMaterial;

    private Renderer meshRenderer;
    private Material originalMataterial;

    private static float duration = 0.15f;

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        originalMataterial = meshRenderer.material;
    }
    
    public void ShowDamage()
    {
        meshRenderer.material = damageMaterial;
        StartCoroutine(nameof(DisableColorAfterTime));
    }

    IEnumerator DisableColorAfterTime()
    {
        yield return new WaitForSeconds(duration);
        meshRenderer.material = originalMataterial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainRegenerator : MonoBehaviour
{
    public GameObject HealSoundPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var colliderHasMainCharMovementController = other.TryGetComponent(typeof(MainCharMovementController), out var mainCharMovementControllerAsComponent);

        if (colliderHasMainCharMovementController)
        {
            var canHeal = (ICanHeal)mainCharMovementControllerAsComponent;

            canHeal.RecoverFullHealth();

            var sound = Instantiate(HealSoundPrefab);
            Destroy(sound, 2.0f);
        }
    }
}

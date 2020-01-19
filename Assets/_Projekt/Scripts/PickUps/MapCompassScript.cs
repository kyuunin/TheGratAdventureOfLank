using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompassScript : MonoBehaviour
{
    public enum Type { MAP, COMPASS };

    public Type type = Type.MAP;

    public GameObject collectSoundPrefab;

    private float angle = 0;
    void Update()
    {
        angle += Time.deltaTime * 360f * 0.5f;
        transform.rotation = Quaternion.Euler(-90, angle, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (type == Type.MAP)
                GameObject.FindObjectOfType<LevelGen>().ActiveMap();
            //if (type == Type.COMPASS)
            // TODO: do something with compass

            var sound = Instantiate(collectSoundPrefab);
            Destroy(sound, 2.0f);

            Destroy(gameObject, 0.05f);
        }
    }
}
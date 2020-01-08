using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPortalStats : MonoBehaviour
{
    public bool LogCameraCount = true;

    void Update()
    {
        if (LogCameraCount)
        {
            int cameraCount = 0;
            int cameraEnabledCount = 0;
            foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
            {
                cameraCount++;
                if (c.enabled) cameraEnabledCount++;
            }
            Debug.Log("" + cameraEnabledCount + " / " + cameraCount + " enabled.");
        }
    }

}

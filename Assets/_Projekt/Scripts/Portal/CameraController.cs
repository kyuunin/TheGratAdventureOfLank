using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera PlayerCamera;
    public Plane PlayerPlane;
    public Plane HiddenPlane;

    public Camera thisCamera;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        var Rot = Quaternion.LookRotation(-HiddenPlane.Normal) * Quaternion.Inverse(Quaternion.LookRotation(PlayerPlane.Normal));

        var RelPlayerPos = PlayerCamera.transform.localPosition - PlayerPlane.Center;
        var RelHiddenPos = Rot * RelPlayerPos;
        transform.localPosition = RelHiddenPos + HiddenPlane.Center;
        transform.localRotation = Rot * PlayerCamera.transform.localRotation;

        thisCamera.fieldOfView = PlayerCamera.fieldOfView;
    }
}

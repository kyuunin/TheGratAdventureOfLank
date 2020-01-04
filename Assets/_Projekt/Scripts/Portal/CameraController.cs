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

        var RelPlayerPos = PlayerCamera.transform.position - PlayerPlane.Center;
        var RelHiddenPos = Rot * RelPlayerPos;
        transform.position = RelHiddenPos + HiddenPlane.Center;
        transform.rotation = Rot * PlayerCamera.transform.rotation;
        //Debug.Log(this.GetHashCode() + ":"+PlayerCamera.transform.position + RelPlayerPos+"\n" + transform.position + RelHiddenPos+"\n"+Rot.eulerAngles);

        thisCamera.fieldOfView = PlayerCamera.fieldOfView;
    }
}

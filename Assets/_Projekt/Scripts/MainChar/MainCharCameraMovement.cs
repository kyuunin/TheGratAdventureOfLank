using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharCameraMovement : MonoBehaviour
{
    public Camera movementCamera;
    public Transform cameraFocus;

    public  float pitch = 0.0f;
    public float cameraCollisionOffset = 0.1f;

    public float mouseSpeed = 5.0f;

    Room lastCurrentRoom = null;
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0, Time.deltaTime * mouseSpeed * mouseX * 180 / Mathf.PI, 0);

        pitch -= Time.deltaTime * mouseSpeed * mouseY;
        while (pitch > Mathf.PI / 2) pitch = Mathf.PI / 2 - 0.001f;
        while (pitch < -Mathf.PI / 2) pitch = -Mathf.PI / 2 + 0.001f;
        
        var cameraVector = transform.rotation * Quaternion.Euler(-pitch * 180 / Mathf.PI, 0, 0) * new Vector3(0,0,1) ;

        float distance = 5;
        float currentDistance = distance - cameraCollisionOffset;

        Plane plane = null;
        RaycastHit hit;
        if(Physics.Raycast(cameraFocus.position, cameraVector, out hit, distance, ~4)) {
            plane = hit.collider.gameObject.GetComponent<Plane>();
            if (plane == null) currentDistance = hit.distance;
        }

        movementCamera.transform.position = cameraFocus.position + cameraVector * currentDistance;
        movementCamera.transform.rotation = transform.rotation * Quaternion.Euler(pitch * 180 / Mathf.PI, 180,0 );

        if (plane != null)
        {
            lastCurrentRoom = plane.Parent;
            plane.Brother.Parent.SetRoomActiveExclusively();
            var Rot = Quaternion.LookRotation(-plane.Brother.Normal) * Quaternion.Inverse(Quaternion.LookRotation(plane.Normal));

            var relCamPos = movementCamera.transform.position - plane.Center;
            var RelHiddenPos = Rot * relCamPos;
            movementCamera.transform.position = RelHiddenPos + plane.Brother.Center;
            movementCamera.transform.rotation = Rot * movementCamera.transform.rotation;
        }
        else Room.CurrentPlayerRoom.SetRoomActiveExclusively();
    }
}

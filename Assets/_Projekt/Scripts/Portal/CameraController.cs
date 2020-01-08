using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera ParentCamera;
    public Plane PlayerPlane;
    public Plane HiddenPlane;

    public Camera thisCamera;

    private bool cullingIsVisible;
    public bool CullingIsVisible
    {
        get
        {
            UpdateCulling();
            return cullingIsVisible;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }
    
    void Update()
    {
        UpdateCulling();
    }

    private void UpdateTransform()
    {
        // HiddenPos: space containing this camera (after portal)
        // PlayerPos: space containing the parent camera (before portal)

        var Rot = Quaternion.LookRotation(-HiddenPlane.Normal) * Quaternion.Inverse(Quaternion.LookRotation(PlayerPlane.Normal));

        var RelPlayerPos = ParentCamera.transform.position - PlayerPlane.Center;
        var RelHiddenPos = Rot * RelPlayerPos;
        transform.position = RelHiddenPos + HiddenPlane.Center;
        transform.rotation = Rot * ParentCamera.transform.rotation;
        //Debug.Log(this.GetHashCode() + ":"+PlayerCamera.transform.position + RelPlayerPos+"\n" + transform.position + RelHiddenPos+"\n"+Rot.eulerAngles);

        thisCamera.fieldOfView = ParentCamera.fieldOfView;
    }
    

    // remember current frame to do frustum culling only once every frame
    private int lastCullingFrame = 0;
    
    private void UpdateCulling()
    {
        if(lastCullingFrame < Time.frameCount)
        {
            lastCullingFrame = Time.frameCount;

            UpdateTransform();

            var cc = ParentCamera.GetComponent<CameraController>();
            cullingIsVisible = cc ? cc.CullingIsVisible : true;
            cullingIsVisible = cullingIsVisible &&
                Vector3.Dot(PlayerPlane.Normal, ParentCamera.transform.forward) < 0;

            if(cullingIsVisible)
            {
                var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(ParentCamera);

                for (int i = 0; i < 5; i++) // left, right, down, up, near; ignore far plane
                    cullingIsVisible = cullingIsVisible && frustumPlanes[i].GetSide(PlayerPlane.Center);
            }

            thisCamera.enabled = cullingIsVisible;

        }

    }


}

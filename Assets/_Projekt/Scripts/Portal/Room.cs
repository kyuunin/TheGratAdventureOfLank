using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isFirst { get; set; } = false;
    public Plane[] planes;
    public GameObject[] Cam { get; set; }
    public Material[] Mats { get; set; }
    public bool Entered { get; set; }
    public Camera mainCamera { get; set; }
    public GameObject CamPrefeb { get; set; }
    public Shader DefaultShader { get; set; }
    public static Room CurrentPlayerRoom { get; set; }
    private static Room lastRoomActive = null;

    public void SetRoomActiveExclusively()
    {
        if (lastRoomActive == this) return;

        if (lastRoomActive) lastRoomActive.SetRoomActive(false);
        lastRoomActive = this;
        this.SetRoomActive(true);
    }

    private void SetRoomActive(bool state)
    {
        foreach (Plane plane in planes)
        {
            plane.cam.SetActive(state);
        }
    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO add some conditions
        if (!Entered)
        {
            Entered = true;
            Cam = new GameObject[planes.Length];
            Mats = new Material[planes.Length];
            for (var i = 0; i < planes.Length; ++i)
            {
                Cam[i] = Object.Instantiate(CamPrefeb);
                var CamCont = Cam[i].GetComponent<CameraController>();
                CamCont.HiddenPlane = planes[i].Brother;
                planes[i].cam = Cam[i];
                CamCont.PlayerPlane = planes[i];
                CamCont.PlayerCamera = mainCamera;
                var Tex = new RenderTexture(Screen.width, Screen.height, 24);
                Cam[i].GetComponent<Camera>().targetTexture = Tex;
                Mats[i] = new Material(DefaultShader) { mainTexture = Tex };
                planes[i].GetComponent<MeshRenderer>().material = Mats[i];
                Cam[i].SetActive(isFirst);
            }
        }
    }

}

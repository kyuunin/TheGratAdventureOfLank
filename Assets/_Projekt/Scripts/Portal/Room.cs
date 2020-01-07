﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isFirst = false;
    public Plane[] planes;
    public GameObject[] Cam { get; set; }
    public Material[] Mats { get; set; }
    public bool Entered { get; set; }
    public Camera mainCamera;
    public GameObject CamPrefeb;
    public Shader DefaultShader;

    public static Room CurrentPlayerRoom { get; set; }

    private static Room lastRoomActive = null;
    public void SetRoomActiveExclusively()
    {
        if (lastRoomActive == this) return;

        if(lastRoomActive) lastRoomActive.SetRoomActive(false);
        lastRoomActive = this;
        this.SetRoomActive(true);
    }

    private void SetRoomActive(bool state)
    {
        // dynamic occlusion culling
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = state;
        foreach (var p in planes)
            foreach (var r in p.Brother.Parent.GetComponentsInChildren<Renderer>())
                r.enabled = state;
        
        // enable/disable portal cameras
        foreach (Plane plane in planes)
        {
            plane.cam.SetActive(state);
        }
    }
    
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


        // dynamic occlusion culling: start level disabled
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
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
            for (var i = 0; i< planes.Length; ++i)
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

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
    public static readonly int RenderDepth = 4;

    // active room list; currentActiveRooms[0] is current Room
    struct ActiveRoomEntry { public Room room; public Camera parentCam; };
    private static List<ActiveRoomEntry> currentActiveRooms = new List<ActiveRoomEntry>();

    public void SetRoomActiveExclusively()
    {
        // already in this room
        if (currentActiveRooms.Count >= 1 && currentActiveRooms[0].room == this)
            return;
        
        // disable old rooms
        foreach (var r in currentActiveRooms)
            r.room.SetRoomActive(false);

        // add root room with mainCamera
        currentActiveRooms.Clear();
        currentActiveRooms.Add(new ActiveRoomEntry { room = this, parentCam = mainCamera });
        
        // recursively add rooms
        for(int i = 1; i < RenderDepth; i++)
        {
            foreach(var e in new List<ActiveRoomEntry>(currentActiveRooms))
            {
                Room r = e.room;
                foreach(var p in r.planes)
                {
                    currentActiveRooms.Add(new ActiveRoomEntry { room = p.Brother.Parent, parentCam = p.cam.GetComponent<Camera>() });
                }
            }
        }

        // enable rooms in reverse order to give parent rooms the highest priority on parentCameras for the portals
        for(int i = currentActiveRooms.Count - 1; i >= 0; i--)
        {
            currentActiveRooms[i].room.SetRoomActive(true, currentActiveRooms[i].parentCam);
        }
        
    }

    private void SetRoomActive(bool state, Camera parentCamera = null)
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
        
        // set parent camera
        
        if(state == true)
            foreach (Plane plane in planes)
                plane.cam.GetComponent<CameraController>().ParentCamera = parentCamera ?? mainCamera;
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
            for (var i = 0; i < planes.Length; ++i)
            {
                Cam[i] = Object.Instantiate(CamPrefeb, transform);
                Cam[i].name = "Portal Cam " + i;
                var CamCont = Cam[i].GetComponent<CameraController>();
                CamCont.HiddenPlane = planes[i].Brother;
                planes[i].cam = Cam[i];
                CamCont.PlayerPlane = planes[i];
                CamCont.ParentCamera = mainCamera;
                var Tex = new RenderTexture(Screen.width, Screen.height, 24);
                Cam[i].GetComponent<Camera>().targetTexture = Tex;
                Mats[i] = new Material(DefaultShader) { mainTexture = Tex };
                planes[i].GetComponent<MeshRenderer>().material = Mats[i];
                Cam[i].SetActive(isFirst);
            }
        }
    }

}

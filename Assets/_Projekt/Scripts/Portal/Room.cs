using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Plane[] planes;
    public GameObject[] Cam { get; set; }
    public Material[] Mats { get; set; }
    public bool Entered { get; set; }
    public Camera mainCamera;
    public GameObject CamPrefeb;
    public Shader DefaultShader;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
                CamCont.HiddenPlane = planes[i].Other;
                CamCont.PlayerPlane = planes[i];
                CamCont.PlayerCamera = mainCamera;
                var Tex = new RenderTexture(Screen.width, Screen.height, 24);
                Cam[i].GetComponent<Camera>().targetTexture = Tex;
                Mats[i] = new Material(DefaultShader) { mainTexture = Tex };
                planes[i].GetComponent<MeshRenderer>().material = Mats[i];
                Cam[i].SetActive(true);
            }
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane: MonoBehaviour
{
    public Vector3 Center { get; set; }
    public Vector3 Normal { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Debug.Log(Center);
        Debug.Log(Normal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init() {
        Center = transform.localPosition;
        Normal = transform.localRotation * new Vector3(0,1,0);
    }
}

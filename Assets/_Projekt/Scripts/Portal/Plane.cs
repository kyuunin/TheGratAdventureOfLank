using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public Vector3 Center { get; set; }
    public Vector3 Normal { get; set; }
    public Plane Other;


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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name+" entered");
        other.transform.rotation = Quaternion.LookRotation(Other.Normal);
        var cc = other.GetComponent<CharacterController>();
        cc.enabled = false;
        other.transform.position = Other.Center;
        cc.enabled = true;
        
    }

    void Init() {
        Center = transform.position;
        Normal = transform.rotation * new Vector3(0,1,0);
    }
}

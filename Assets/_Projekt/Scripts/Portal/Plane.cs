using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public Vector3 Center { get; set; }
    public Vector3 Normal { get; set; }
    public Plane Brother;


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
    private void OnTriggerExit(Collider other)
    {
        var cc = other.GetComponent<CharacterController>();
        
        var pos = other.transform.TransformPoint(cc.center)-Center;
        var dir = Vector3.Dot(pos, Normal);
        
        if(dir<0)
        {
            cc.enabled = false;
            {
                Debug.Log(other.name + " entered");

                var Rot = Quaternion.LookRotation(-Brother.Normal) * Quaternion.Inverse(Quaternion.LookRotation(Normal));

                var RelPlayerPos = other.transform.position - Center;
                var RelHiddenPos = Rot * RelPlayerPos;
                other.transform.position = RelHiddenPos + Brother.Center;
                other.transform.rotation = Rot * other.transform.rotation;
            }
            cc.enabled = true;
        }
        
    }
    

    void Init() {
        Center = transform.position;
        Normal = transform.rotation * new Vector3(0,1,0);
    }
}

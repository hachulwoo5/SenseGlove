using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabable : MonoBehaviour
{ 
    public bool isGrabbed;

    Rigidbody rigid;

 
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    
    
}
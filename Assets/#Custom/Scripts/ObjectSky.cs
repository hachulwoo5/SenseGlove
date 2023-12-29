using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSky : MonoBehaviour
{
    Rigidbody rigid;
    public bool isSky;
    public int isNumber;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isNumber>=3)
        {
            isSky = true;
        }
        if(isSky==true)
        {
            rigid.useGravity = true;
            isSky = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag=="Sphere")
        {
            isNumber++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sphere")
        {
            isNumber--;
        }
    }
}

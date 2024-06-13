using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMani : MonoBehaviour
{
    public int a;
    private void OnTriggerEnter ( Collider other )
    {
        
       if ( other.gameObject.name== this.gameObject.name)
       {
           a++;
       }
       
       
    }
}

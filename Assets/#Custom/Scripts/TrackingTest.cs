using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTest : MonoBehaviour
{
    public GameObject obj;
    Rigidbody rigid;

    private void Awake ( )
    {
      rigid=   GetComponent<Rigidbody> ( );
    }
    private void FixedUpdate ( )
    {
        this. transform. position = obj. transform. position;
        this. transform. rotation = obj. transform. rotation;
    }
    private void OnTriggerEnter ( Collider other )
    {
        if ( other. gameObject. tag == "Wall" )
        {
            Debug. Log ( "asd" );
            this. rigid. velocity = Vector3. zero;

        }
    }
    private void OnCollisionEnter ( Collision other )
    {
        if ( other.gameObject.tag == "Wall" )
        {
            Debug. Log ( "asd" );
            this. rigid. velocity = Vector3. zero;

        }
    }
}

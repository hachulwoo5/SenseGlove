using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNew : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    Rigidbody rigid;
    public Transform target;
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider> ( );
        rigid = GetComponent<Rigidbody> ( );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate ( )
    {
        rigid. velocity = ( target. position - transform. position ) / Time. fixedDeltaTime;
    }

    private void OnTriggerStay ( Collider other )
    {
        rigid. velocity = Vector3. zero;

    }
}

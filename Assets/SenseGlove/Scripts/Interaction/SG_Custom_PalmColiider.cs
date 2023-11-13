using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
    public class SG_Custom_PalmColiider : MonoBehaviour
    {

        public GameObject GrabLayerObj;
        public void OnTriggerEnter ( Collider other )
        {
            if ( other. CompareTag ( "Obj" )  )
            {
                GrabLayerObj. GetComponent<SG_PhysicsGrab> ( ). isPalmTouch = true;
            }
        }
        public void OnTriggerStay ( Collider other )
        {
            if ( other. CompareTag ( "Obj" ) )
            {
                GrabLayerObj. GetComponent<SG_PhysicsGrab> ( ). isPalmTouch = true;
            }
        }
        public void OnTriggerExit ( Collider other )
        {
            if ( other. CompareTag ( "Obj" ) )
            {
                GrabLayerObj. GetComponent<SG_PhysicsGrab> ( ). isPalmTouch = false;
            }
        }
    }

}

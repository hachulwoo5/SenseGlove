using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System. Linq;

public class Master_VR : MonoBehaviour
{
    public bool [ ] Checklist = new bool [ 6 ];

    public ParentObject_VR palmTouch;
    public ParentObject_VR thumbTouch;
    public ParentObject_VR indexTouch;
    public ParentObject_VR middleTouch;
    public ParentObject_VR ringTouch;
    public ParentObject_VR pinkyTouch;


    public int pointIndex = 0;
    public float objMass;

    public GameObject grabbedObject;
    public Transform handTransform;

    public List<ParentObject_VR> childScripts = new List<ParentObject_VR> ( );

    public bool isGrabbing;

    private void Awake ( )
    {
        childScripts. AddRange ( GetComponentsInChildren<ParentObject_VR> ( ) );

    }
    private void Update ( )
    {
        CheckSection ( );
        EvaluateGrab ( );
        EvaluateRelease ( );
    }

    public void EvaluateGrab ( )
    {

        if ( pointIndex >= 1 )
        {
            
            if( grabbedObject ==null)
            {
                grabbedObject = thumbTouch. grabbedObj_p;
            }
            grabbedObject = thumbTouch. grabbedObj_p;
            grabbedObject. GetComponent<ObjectGrabable_VR> ( ). isGrabbed = true;
            grabbedObject. GetComponent<ObjectGrabable_VR> ( ). grabbingHand = handTransform;
            //   grabbedObject.GetComponent<ObjectGrabable>().initialGrabHandPosition = handTransform.position;
            grabbedObject. GetComponent<ObjectGrabable_VR> ( ). offset = handTransform. position - grabbedObject. transform. position;


            //  grabbedObject.GetComponent<ObjectGrabable>().lastHandPosition = handTransform.position;
            //  grabbedObject.GetComponent<ObjectGrabable>().lastHandRotation = handTransform.eulerAngles;


        }
    }
    public void EvaluateRelease ( )
    {
        if ( grabbedObject != null )
        {
            if ( pointIndex < 1 )
            {
                //   Debug.Log(grabbedObject.GetComponent<Rigidbody>().velocity);
                grabbedObject. GetComponent<ObjectGrabable_VR> ( ). isGrabbed = false;
                grabbedObject = null;
            }
        }
    }
    void CheckSection ( )
    {
        Checklist [ 0 ] = palmTouch. isReadyGrab;
        Checklist [ 1 ] = thumbTouch. isReadyGrab ;
        Checklist [ 2 ] = indexTouch. isReadyGrab ;
        Checklist [ 3 ] = middleTouch. isReadyGrab;
        Checklist [ 4 ] = ringTouch. isReadyGrab;
        Checklist [ 5 ] = pinkyTouch. isReadyGrab;

        

        pointIndex = Checklist. Count ( value => value );

        List<float> fingerMasses = new List<float>
            {
            thumbTouch.objMass,
            indexTouch.objMass,
            middleTouch.objMass,
            ringTouch.objMass,
            pinkyTouch.objMass,
            };
        float FindObjectMass ( )
        {
            var nonZeroMasses = fingerMasses. Where ( mass => mass != 0f );
            var mostFrequentMass = nonZeroMasses. GroupBy ( mass => mass )
                                                . OrderByDescending ( group => group. Count ( ) )
                                                . Select ( group => group. Key )
                                                . FirstOrDefault ( );

            return mostFrequentMass;
        }
        objMass = FindObjectMass ( );
    }
}

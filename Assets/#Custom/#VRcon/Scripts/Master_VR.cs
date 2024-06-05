using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System. Linq;
using Valve. VR;

    
    public class Master_VR : MonoBehaviour
{
    public SteamVR_Behaviour_Skeleton SkeleltonInformation;
    public bool [ ] Checklist = new bool [ 6 ];

    public ParentObject_VR thumbTouch;
    public ParentObject_VR indexTouch;
    public ParentObject_VR middleTouch;
    public ParentObject_VR ringTouch;
    public ParentObject_VR pinkyTouch;
    public ParentObject_VR palmTouch;

    public int pointIndex = 0;
    public float objMass;

    public GameObject grabbedObject;
    public Transform handTransform;

    public List<ParentObject_VR> childScripts = new List<ParentObject_VR> ( );

    public bool isGrabbing;

    public float thumbC;
    public float indexC;

    float maxThumbC;
    private void Awake ( )
    {
        childScripts. AddRange ( GetComponentsInChildren<ParentObject_VR> ( ) );

    }
    private void Update ( )
    {
        thumbC = SkeleltonInformation. thumbCurl;
        indexC = SkeleltonInformation. indexCurl;
        CheckSection ( ); 
        EvaluateGrab ( );
        EvaluateRelease ( );  


    }

    public void EvaluateGrab ( )
    {
        

        if ( pointIndex >= 2 && thumbTouch.isReadyGrab )
        {
            // 각 손가락에서 감지된 오브젝트 리스트
            List<GameObject> detectedObjects = new List<GameObject> ( );

            // Thumb 손가락
            if ( thumbTouch. grabbedObj_p != null )
                detectedObjects. Add ( thumbTouch. grabbedObj_p );
            // Index 손가락
            if ( indexTouch. grabbedObj_p != null )
                detectedObjects. Add ( indexTouch. grabbedObj_p );
            // Middle 손가락
            if ( middleTouch. grabbedObj_p != null )
                detectedObjects. Add ( middleTouch. grabbedObj_p );
            // Ring 손가락
            if ( ringTouch. grabbedObj_p != null )
                detectedObjects. Add ( ringTouch. grabbedObj_p );
            // Pinky 손가락
            if ( pinkyTouch. grabbedObj_p != null )
                detectedObjects. Add ( pinkyTouch. grabbedObj_p );

            // 감지된 오브젝트가 하나라도 있으면 그 중 가장 첫 번째 오브젝트를 grabbedObject로 할당
            if ( detectedObjects. Count > 0 )
            {
                grabbedObject = detectedObjects [ 0 ];
                //grabbedObject. GetComponent<ObjectGrabable_VR> ( ). isGrabbed = true;
               // grabbedObject. GetComponent<ObjectGrabable_VR> ( ). grabbingHand = handTransform;
               // grabbedObject. GetComponent<ObjectGrabable_VR> ( ). offset = handTransform. position - grabbedObject. transform. position;
            }
            /*
            if ( grabbedObject == null )
            {
                grabbedObject = thumbTouch. grabbedObj_p;
            }
            grabbedObject = thumbTouch. grabbedObj_p;
            grabbedObject. GetComponent<ObjectGrabable_VR> ( ). isGrabbed = true;*/
            //  
            //   grabbedObject.GetComponent<ObjectGrabable>().initialGrabHandPosition = handTransform.position;
            //


            //  grabbedObject.GetComponent<ObjectGrabable>().lastHandPosition = handTransform.position;
            //  grabbedObject.GetComponent<ObjectGrabable>().lastHandRotation = handTransform.eulerAngles;


        }
    }
    public void EvaluateRelease ( )
    {
        if ( grabbedObject != null )
        {
            if ( pointIndex < 2 || SkeleltonInformation. thumbCurl < 0.3f )
            {
                //   Debug.Log(grabbedObject.GetComponent<Rigidbody>().velocity);
               // EnableCapsuleCollidersRecursively ( transform );
               // grabbedObject. GetComponent<ObjectGrabable_VR> ( ). isGrabbed = false;
                grabbedObject = null;
            }
        }
    }
    void CheckSection ( )
    {
        Checklist [ 0 ] = thumbTouch. isReadyGrab;
        Checklist [ 1 ] = indexTouch. isReadyGrab;
        Checklist [ 2 ] = middleTouch. isReadyGrab;
        Checklist [ 3 ] = ringTouch. isReadyGrab;
        Checklist [ 4 ] = pinkyTouch. isReadyGrab;
        Checklist [ 5 ] = palmTouch. isReadyGrab;


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

        private void DisableCapsuleCollidersRecursively ( Transform parent )
        {
            foreach ( Transform child in parent )
            {
                // 캡슐 콜라이더를 가지고 있는 경우 비활성화합니다.
                CapsuleCollider capsuleCollider = child. GetComponent<CapsuleCollider> ( );
                if ( capsuleCollider != null )
                {
                    capsuleCollider. enabled = false;
                }

                // 하위 오브젝트에 대해 재귀적으로 호출합니다.
                DisableCapsuleCollidersRecursively ( child );
            }
        }
    private void EnableCapsuleCollidersRecursively ( Transform parent )
    {
        foreach ( Transform child in parent )
        {
            // 캡슐 콜라이더를 가지고 있는 경우 활성화합니다.
            CapsuleCollider capsuleCollider = child. GetComponent<CapsuleCollider> ( );
            if ( capsuleCollider != null )
            {
                capsuleCollider. enabled = true;
            }

            // 하위 오브젝트에 대해 재귀적으로 호출합니다.
            EnableCapsuleCollidersRecursively ( child );
        }
    }

}



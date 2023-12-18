using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class ParentObject_Custom : MonoBehaviour
{
    public float beingTouched = 0;
    public float touchPercentage;
    public bool isGrabbing;
    public List<ChildObject_Custom> childScripts = new List<ChildObject_Custom> ( );

    public Transform handTransform;
    public Transform grabLayer;

    private GameObject grabbedObject;

    // 추가 변수
    private List<Vector3> grabLayerPositionHistory = new List<Vector3> ( );

    private void Start ( )
    {
        childScripts. AddRange ( GetComponentsInChildren<ChildObject_Custom> ( ) );

        foreach ( ChildObject_Custom childScript in childScripts )
        {
            childScript. OnColorChanged += HandleColorChanged;
        }

        InvokeRepeating ( "LogBeingTouchedRatio" , 0f , 1.2f );
    }

    private void LogBeingTouchedRatio ( )
    {
        // 로그 출력
        // Debug.Log(this.transform.name + "터치 중인 비율 = " + (beingTouched / childScripts.Count) * 100f + " % ");
    }

    public void HandleColorChanged ( Color newColor , GameObject other )
    {
        if ( newColor == Color. green )
        {
            beingTouched++;
        }
        if ( newColor == Color. red )
        {
            beingTouched--;
        }

        touchPercentage = beingTouched / childScripts. Count;

        List<ChildObject_Custom> matchingScripts = childScripts. FindAll ( script => script. grabbedObj == other && other != null );
        if ( matchingScripts. Count >= 2 )
        {
            grabbedObject = other;
            grabbedObject. transform. SetParent ( grabLayer );
            grabbedObject. GetComponent<Rigidbody> ( ). isKinematic = true;

            isGrabbing = true;
        }
        else
        {
            ReleaseGrabbedObject ( );
        }
    }

    private void FixedUpdate ( )
    {
        

    }

    public void EvaluateGrab ( )
    {
        
    }

    public void ReleaseGrabbedObject ( )
    {
        if ( grabbedObject != null )
        {
            grabbedObject. transform. SetParent ( null );
            grabbedObject. GetComponent<ObjectGrabable> ( ). isGrabbed = false;
            grabbedObject. GetComponent<Rigidbody> ( ). isKinematic = false;

            // 평균 속도 계산 및 부여
            Vector3 averageVelocity = CalculateAverageVelocity ( );
            grabbedObject. GetComponent<Rigidbody> ( ). velocity = averageVelocity;

            
            grabbedObject = null;
            isGrabbing = false;
        }
    }

    private Vector3 CalculateAverageVelocity ( )
    {
        if ( grabLayerPositionHistory. Count < 2 )
            return Vector3. zero;

        Vector3 totalDisplacement = Vector3. zero;
        for ( int i = 1 ; i < grabLayerPositionHistory. Count ; i++ )
        {
            totalDisplacement += grabLayerPositionHistory [ i ] - grabLayerPositionHistory [ i - 1 ];
        }

        // 평균 속도 계산
        Vector3 averageVelocity = totalDisplacement / ( grabLayerPositionHistory. Count - 1 );
        return averageVelocity;
    }

    private void UpdateGrabLayerPositionHistory ( )
    {
        if ( grabLayer != null )
        {
            grabLayerPositionHistory. Add ( grabLayer. position );

            // 이력 개수 제한
            if ( grabLayerPositionHistory. Count > 10 )
            {
                grabLayerPositionHistory. RemoveAt ( 0 );
            }
        }
    }
}
using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class ObjectGrabable_VR : MonoBehaviour
{
    public bool isGrabbed;

    Rigidbody rigid;

    public Transform grabbingHand;
    public Vector3 lastHandPosition;
    public Vector3 lastHandRotation;

    public Vector3 initialGrabHandPosition; // ���� �׷� �� �� ��ġ
    public Vector3 offset; // �հ� ��ü ���� ��� ��ġ

    public int frameCount = 10; // �ε巯�� �ӷ��� ����� ������ ��
    public List<Vector3> positionHistory = new List<Vector3> ( );

    private bool releasedDebug = false; // ������ ����׸� ����� ���θ� ��Ÿ���� �÷���

    public Transform originTransform;
    private void Awake ( )
    {
        rigid = GetComponent<Rigidbody> ( );

        originTransform = this. transform. parent;
    }

    void FixedUpdate ( )
    {
        if ( isGrabbed )
        {
            
            UpdateLocation ( );
           
        }
        else
        {
            NoGrabbing ( );
        }
    }

    public void UpdateLocation ( )
    {
        rigid. useGravity = false;
        rigid. isKinematic = true;
        this. transform. parent = grabbingHand. transform; RecordPosition ( );
        // ������� �������� ����� ���
        if ( releasedDebug )
        {
            Debug. Log ( "Release Velocity: " + rigid. velocity. magnitude );
            releasedDebug = false; // �÷��� ����
        }

        // transform. position = Vector3. MoveTowards ( transform. position , grabbingHand. position - offset , 3f * 0.5f );
        // transform. rotation = grabbingHand. rotation;

    }

    public void NoGrabbing ( )
    {
        rigid. isKinematic = false;
        rigid. useGravity = true;
        this. transform. parent = originTransform;
        // ������� ������ �� ���� ����� ���
        if ( !releasedDebug )
        {
            Debug. Log ( "Released" );

            releasedDebug = true; // �÷��� ����
            rigid. velocity = SmoothVelocity*100f;

        }
    }

    public Vector3 SmoothVelocity
    {
        get
        {
            if ( positionHistory. Count < 2 )
                return Vector3. zero;

            Vector3 totalDisplacement = Vector3. zero;
            for ( int i = 1 ; i < positionHistory. Count ; i++ )
            {
                totalDisplacement += positionHistory [ i ] - positionHistory [ i - 1 ];
            }
            return totalDisplacement / ( frameCount - 1 );
        }
    }

    private void RecordPosition ( )
    {
        positionHistory. Add ( transform. position );

        if ( positionHistory. Count > frameCount )
        {
            positionHistory. RemoveAt ( 0 );
        }
    }
}
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

    public Vector3 initialGrabHandPosition; // 최초 그랩 시 손 위치
    public Vector3 offset; // 손과 물체 간의 상대 위치

    public int frameCount = 10; // 부드러운 속력을 계산할 프레임 수
    public List<Vector3> positionHistory = new List<Vector3> ( );

    private bool releasedDebug = false; // 릴리즈 디버그를 출력한 여부를 나타내는 플래그

    private void Awake ( )
    {
        rigid = GetComponent<Rigidbody> ( );
    }

    void FixedUpdate ( )
    {
        if ( isGrabbed )
        {
            UpdateLocation ( );
            RecordPosition ( );
            Vector3 smoothVelocity = SmoothVelocity;
            rigid. velocity = smoothVelocity;

            // 릴리즈된 순간에만 디버그 출력
            if ( releasedDebug )
            {
                Debug. Log ( "Release Velocity: " + rigid. velocity. magnitude );
                releasedDebug = false; // 플래그 리셋
            }
        }
        else
        {
            NoGrabbing ( );
        }
    }

    public void UpdateLocation ( )
    {
        rigid. useGravity = false;
        transform. position = Vector3. MoveTowards ( transform. position , grabbingHand. position - offset , 3f * 0.5f );
        transform. rotation = grabbingHand. rotation;
    }

    public void NoGrabbing ( )
    {
        rigid. useGravity = true;

        // 릴리즈된 순간에 한 번만 디버그 출력
        if ( !releasedDebug )
        {
            Debug. Log ( "Released" );
            releasedDebug = true; // 플래그 설정
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
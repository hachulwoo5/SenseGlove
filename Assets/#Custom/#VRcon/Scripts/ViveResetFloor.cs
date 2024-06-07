using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class ViveResetFloor : MonoBehaviour
{
    private Dictionary<GameObject , Vector3> initialPositions; // 초기 위치를 저장할 딕셔너리
    public GameObject [ ] ResetObjectList;

    void Start ( )
    {
        initialPositions = new Dictionary<GameObject , Vector3> ( );
        for ( int i = 0 ; i < ResetObjectList. Length ; i++ )
        {
            initialPositions [ ResetObjectList [ i ] ] = ResetObjectList [ i ]. transform. position;
        }
    }

    void OnTriggerEnter ( Collider other )
    {
        GameObject obj = other. gameObject;

        // 오브젝트가 ResetObjectList에 있는지 확인합니다.
        if ( initialPositions. ContainsKey ( obj ) )
        {
            // 오브젝트를 초기 위치로 되돌립니다.
            obj. transform. position = initialPositions [ obj ];

            // 오브젝트의 Rigidbody를 가져와서 속도와 방향을 초기화합니다.
            Rigidbody rb = obj. GetComponent<Rigidbody> ( );
            if ( rb != null )
            {
                rb. velocity = Vector3. zero;
                rb. angularVelocity = Vector3. zero;
            }
        }
    }
}

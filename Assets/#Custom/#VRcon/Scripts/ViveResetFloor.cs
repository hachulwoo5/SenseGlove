using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class ViveResetFloor : MonoBehaviour
{
    private Dictionary<GameObject , Vector3> initialPositions; // �ʱ� ��ġ�� ������ ��ųʸ�
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

        // ������Ʈ�� ResetObjectList�� �ִ��� Ȯ���մϴ�.
        if ( initialPositions. ContainsKey ( obj ) )
        {
            // ������Ʈ�� �ʱ� ��ġ�� �ǵ����ϴ�.
            obj. transform. position = initialPositions [ obj ];

            // ������Ʈ�� Rigidbody�� �����ͼ� �ӵ��� ������ �ʱ�ȭ�մϴ�.
            Rigidbody rb = obj. GetComponent<Rigidbody> ( );
            if ( rb != null )
            {
                rb. velocity = Vector3. zero;
                rb. angularVelocity = Vector3. zero;
            }
        }
    }
}

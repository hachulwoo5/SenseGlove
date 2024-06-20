using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InObjTestScp : MonoBehaviour
{
    public int substeps = 5; // ���꽺���� ��
    private float subStepDuration;

    void Start ( )
    {
        // ���꽺���� �ð� ������ ����
        subStepDuration = Time. fixedDeltaTime / substeps;
    }

    void FixedUpdate ( )
    {
        // ���꽺���� ����ŭ ���� ������ ����
        for ( int i = 0 ; i < substeps ; i++ )
        {
            Physics. Simulate ( subStepDuration );
        }
    }

}

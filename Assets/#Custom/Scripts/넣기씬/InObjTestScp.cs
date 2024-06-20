using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InObjTestScp : MonoBehaviour
{
    public int substeps = 5; // 서브스텝의 수
    private float subStepDuration;

    void Start ( )
    {
        // 서브스텝의 시간 간격을 설정
        subStepDuration = Time. fixedDeltaTime / substeps;
    }

    void FixedUpdate ( )
    {
        // 서브스텝의 수만큼 물리 연산을 수행
        for ( int i = 0 ; i < substeps ; i++ )
        {
            Physics. Simulate ( subStepDuration );
        }
    }

}

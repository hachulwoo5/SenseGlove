using UnityEngine;
using System. IO;
using UnityEngine. SceneManagement;

public class BoxStackCalculate : MonoBehaviour
{
    public GameObject [ ] TargetCube; // 기준이 되는 큐브
    public GameObject [ ] MoveCube; // 비교할 큐브

    public float Difference;
    public float totalDifference;

    void Start ( )
    {
        if ( TargetCube != null && MoveCube != null )
        {
            MoveCube = new GameObject [ TargetCube. Length ]; // MoveCube 배열 초기화
        }
        else
        {
            Debug. LogError ( "큐브 오브젝트가 할당되지 않았습니다." );
        }
    }

    private void Update ( )
    {
        if ( Input. GetKeyDown ( KeyCode. F9 ) )
        {
            AssignMoveCubes ( );
            totalDifference = 0;
            for ( int i = 0 ; i < TargetCube. Length ; i++ )
            {
                Difference = 0;
                if ( MoveCube [ i ] != null )
                {
                    Difference = CalculateTotalDifference ( TargetCube [ i ] , MoveCube [ i ] ) * 100;
                    totalDifference += Difference;
                }
                else
                {
                    Debug. LogError ( "MoveCube[" + i + "]이 할당되지 않았습니다." );
                }
            }
            SaveDistanceToFile ( totalDifference );
            Debug. Log ( "Total Difference: " + totalDifference. ToString ( "F2" ) );
        }
    }

    private void SaveDistanceToFile ( float distance )
    {
        string sceneName = SceneManager. GetActiveScene ( ). name; // 현재 씬 이름 가져오기
        string path = $"{sceneName}.txt"; // 파일 경로 지정

        // 저장할 문자열 포맷
        string disString = $"거리 차이: {distance:F2}\n";

        // 파일에 경과 시간 추가
        File. AppendAllText ( path , disString );

        Debug. Log ( "거리차이가 파일에 저장되었습니다: " + path );
    }

    void AssignMoveCubes ( )
    {
        for ( int i = 0 ; i < TargetCube. Length ; i++ )
        {
            TargetBox targetBox = TargetCube [ i ]. GetComponent<TargetBox> ( );
            if ( targetBox != null )
            {
                if ( targetBox. MoveCube != null )
                {
                    MoveCube [ i ] = targetBox. MoveCube;
                }
                else
                {
                    Debug. LogError ( "TargetBox.MoveCube가 할당되지 않았습니다: " + TargetCube [ i ]. name );
                }
            }
            else
            {
                Debug. LogError ( "TargetCube에 TargetBox 스크립트가 없습니다: " + TargetCube [ i ]. name );
            }
        }
    }

    float CalculateTotalDifference ( GameObject c1 , GameObject c2 )
    {
        BoxCollider collider1 = c1. GetComponent<BoxCollider> ( );
        BoxCollider collider2 = c2. GetComponent<BoxCollider> ( );

        if ( collider1 == null || collider2 == null )
        {
            Debug. LogError ( "박스 콜라이더가 할당되지 않았습니다." );
            return 0;
        }

        Vector3 [ ] corners1 = GetCorners ( c1 , collider1 );
        Vector3 [ ] corners2 = GetCorners ( c2 , collider2 );

        float totalDifference = 0f;

        foreach ( Vector3 corner1 in corners1 )
        {
            float minDistance = float. MaxValue;

            foreach ( Vector3 corner2 in corners2 )
            {
                float distance = Vector3. Distance ( corner1 , corner2 );
                if ( distance < minDistance )
                {
                    minDistance = distance;
                }
            }

            totalDifference += minDistance;
        }

        return totalDifference;
    }

    Vector3 [ ] GetCorners ( GameObject cube , BoxCollider collider )
    {
        Vector3 [ ] corners = new Vector3 [ 8 ];
        Vector3 center = collider. center;
        Vector3 size = collider. size / 2;

        // 꼭짓점 좌표를 로컬 공간에서 계산
        corners [ 0 ] = center + new Vector3 ( -size. x , -size. y , -size. z );
        corners [ 1 ] = center + new Vector3 ( size. x , -size. y , -size. z );
        corners [ 2 ] = center + new Vector3 ( size. x , size. y , -size. z );
        corners [ 3 ] = center + new Vector3 ( -size. x , size. y , -size. z );
        corners [ 4 ] = center + new Vector3 ( -size. x , -size. y , size. z );
        corners [ 5 ] = center + new Vector3 ( size. x , -size. y , size. z );
        corners [ 6 ] = center + new Vector3 ( size. x , size. y , size. z );
        corners [ 7 ] = center + new Vector3 ( -size. x , size. y , size. z );

        // 로컬 공간의 꼭짓점을 월드 공간으로 변환
        for ( int i = 0 ; i < 8 ; i++ )
        {
            corners [ i ] = cube. transform. TransformPoint ( corners [ i ] );
        }

        return corners;
    }
}

using UnityEngine;
using System. IO;
using UnityEngine. SceneManagement;

public class BoxStackCalculate : MonoBehaviour
{
    public GameObject [ ] TargetCube; // ������ �Ǵ� ť��
    public GameObject [ ] MoveCube; // ���� ť��

    public float Difference;
    public float totalDifference;

    void Start ( )
    {
        if ( TargetCube != null && MoveCube != null )
        {
            MoveCube = new GameObject [ TargetCube. Length ]; // MoveCube �迭 �ʱ�ȭ
        }
        else
        {
            Debug. LogError ( "ť�� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�." );
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
                    Debug. LogError ( "MoveCube[" + i + "]�� �Ҵ���� �ʾҽ��ϴ�." );
                }
            }
            SaveDistanceToFile ( totalDifference );
            Debug. Log ( "Total Difference: " + totalDifference. ToString ( "F2" ) );
        }
    }

    private void SaveDistanceToFile ( float distance )
    {
        string sceneName = SceneManager. GetActiveScene ( ). name; // ���� �� �̸� ��������
        string path = $"{sceneName}.txt"; // ���� ��� ����

        // ������ ���ڿ� ����
        string disString = $"�Ÿ� ����: {distance:F2}\n";

        // ���Ͽ� ��� �ð� �߰�
        File. AppendAllText ( path , disString );

        Debug. Log ( "�Ÿ����̰� ���Ͽ� ����Ǿ����ϴ�: " + path );
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
                    Debug. LogError ( "TargetBox.MoveCube�� �Ҵ���� �ʾҽ��ϴ�: " + TargetCube [ i ]. name );
                }
            }
            else
            {
                Debug. LogError ( "TargetCube�� TargetBox ��ũ��Ʈ�� �����ϴ�: " + TargetCube [ i ]. name );
            }
        }
    }

    float CalculateTotalDifference ( GameObject c1 , GameObject c2 )
    {
        BoxCollider collider1 = c1. GetComponent<BoxCollider> ( );
        BoxCollider collider2 = c2. GetComponent<BoxCollider> ( );

        if ( collider1 == null || collider2 == null )
        {
            Debug. LogError ( "�ڽ� �ݶ��̴��� �Ҵ���� �ʾҽ��ϴ�." );
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

        // ������ ��ǥ�� ���� �������� ���
        corners [ 0 ] = center + new Vector3 ( -size. x , -size. y , -size. z );
        corners [ 1 ] = center + new Vector3 ( size. x , -size. y , -size. z );
        corners [ 2 ] = center + new Vector3 ( size. x , size. y , -size. z );
        corners [ 3 ] = center + new Vector3 ( -size. x , size. y , -size. z );
        corners [ 4 ] = center + new Vector3 ( -size. x , -size. y , size. z );
        corners [ 5 ] = center + new Vector3 ( size. x , -size. y , size. z );
        corners [ 6 ] = center + new Vector3 ( size. x , size. y , size. z );
        corners [ 7 ] = center + new Vector3 ( -size. x , size. y , size. z );

        // ���� ������ �������� ���� �������� ��ȯ
        for ( int i = 0 ; i < 8 ; i++ )
        {
            corners [ i ] = cube. transform. TransformPoint ( corners [ i ] );
        }

        return corners;
    }
}

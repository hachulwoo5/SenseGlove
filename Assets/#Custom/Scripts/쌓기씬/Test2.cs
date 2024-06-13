using UnityEngine;

public class Test2 : MonoBehaviour
{
    public GameObject Targetcube1;
    public GameObject Targetcube2;
    public GameObject Targetcube3;
    public GameObject Targetcube4;
    public GameObject Targetcube5;
    public GameObject Targetcube6;


    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public GameObject cube5;
    public GameObject cube6;

    public float Sector1;
    public float Sector2;
    public float Sector3;
    public float Sector4;
    public float Sector5;
    public float Sector6;

    public float FinalPercent;
    void Update ( )
    {

        Sector1 = CalculateOverlapPercentage ( Targetcube1 , cube1 );
        Sector2 = CalculateOverlapPercentage ( Targetcube2 , cube2 );
        Sector3 = CalculateOverlapPercentage ( Targetcube3 , cube3 );
        Sector4 = CalculateOverlapPercentage ( Targetcube4 , cube4 );
        Sector5 = CalculateOverlapPercentage ( Targetcube5 , cube5 );
        Sector6 = CalculateOverlapPercentage ( Targetcube6 , cube6 );
        FinalPercent = ( Sector1 + Sector2 + Sector3 + Sector4 + Sector5 + Sector6 ) / 6;



        if ( Input. GetKeyDown ( KeyCode. F8 ) )
        {
            Sector1 = CalculateOverlapPercentage ( Targetcube1 , cube1 );
            Sector2 = CalculateOverlapPercentage ( Targetcube2 , cube2 );
            Sector3 = CalculateOverlapPercentage ( Targetcube3 , cube3 );
            Sector4 = CalculateOverlapPercentage ( Targetcube4 , cube4 );
            Sector5 = CalculateOverlapPercentage ( Targetcube5 , cube5 );
            Sector6 = CalculateOverlapPercentage ( Targetcube6 , cube6 );
            FinalPercent = ( Sector1 + Sector2 + Sector3 + Sector4 + Sector5 + Sector6 ) / 6;
            Debug. Log ( Targetcube1 + "%/ " + Targetcube2 + "%/ " + Targetcube3 + "%/ " + Targetcube4 + "%/ " + Targetcube5 + "%/ " + Targetcube6 + "%/ " );
            Debug. Log ( FinalPercent );

        }

    }

    /*
    ��ü ����
    �� ť���� ũ��, ��ġ, ȸ�� �� ��������:
    cube1.transform.localScale ������ �� ť���� ũ��, ��ġ, ȸ���� �����ɴϴ�.

    ��� ���� ����:
    GetBounds �޼��带 ���� �� ť���� ��� ���ڸ� �����մϴ�. �� ��, ȸ���� ť���� ������ ��ǥ�� ����Ͽ� ��� ���ڸ� ����ϴ�.

    ���� ���� ���:
    Bounds.Intersects�� �� ��� ���ڰ� �����ϴ��� Ȯ���մϴ�.
    �����ϴ� ���, GetIntersection���� ���� ������ ��� ���ڸ� ����մϴ�.

    ��ħ�� ���:
    ���� ������ ���Ǹ� ����Ͽ� �� ť�� �� �� ���� ť���� ���Ƿ� ������ ��ħ���� ���մϴ�.
    �������� �ʴ� ��� ��ħ���� 0%�Դϴ�.

�� ��ũ��Ʈ�� ����ϸ� �� ť�갡 �󸶳� ���ƴ��� ���������� Ȯ���� �� �ֽ��ϴ�. ť���� ȸ������ ����Ͽ� ��Ȯ�� ��ħ���� ����ϱ� ������ �پ��� ��Ȳ������ �����ϰ� ����� �� �ֽ��ϴ�.
     */
    float CalculateOverlapPercentage ( GameObject cube1 , GameObject cube2 )
    {
        // Get the size, position, and rotation of each cube
        Vector3 size1 = cube1. transform. localScale;
        Vector3 pos1 = cube1. transform. position;
        Quaternion rot1 = cube1. transform. rotation;

        Vector3 size2 = cube2. transform. localScale;
        Vector3 pos2 = cube2. transform. position;
        Quaternion rot2 = cube2. transform. rotation;

        // Create bounds for each cube
        Bounds bounds1 = GetBounds ( pos1 , size1 , rot1 );
        Bounds bounds2 = GetBounds ( pos2 , size2 , rot2 );

        // Calculate the intersection of the bounds
        if ( bounds1. Intersects ( bounds2 ) )
        {
            Bounds intersection = GetIntersection ( bounds1 , bounds2 );
            float intersectionVolume = intersection. size. x * intersection. size. y * intersection. size. z;
            float cube1Volume = size1. x * size1. y * size1. z;
            float cube2Volume = size2. x * size2. y * size2. z;
            float minVolume = Mathf. Min ( cube1Volume , cube2Volume );

            return ( intersectionVolume / minVolume ) * 100f;
        }
        else
        {
            return 0f;
        }
    }

    Bounds GetBounds ( Vector3 position , Vector3 size , Quaternion rotation )
    {
        // Calculate the corners of the rotated cube
        Vector3 [ ] corners = new Vector3 [ 8 ];
        Vector3 halfSize = size / 2f;
        corners [ 0 ] = position + rotation * new Vector3 ( -halfSize. x , -halfSize. y , -halfSize. z );
        corners [ 1 ] = position + rotation * new Vector3 ( halfSize. x , -halfSize. y , -halfSize. z );
        corners [ 2 ] = position + rotation * new Vector3 ( halfSize. x , -halfSize. y , halfSize. z );
        corners [ 3 ] = position + rotation * new Vector3 ( -halfSize. x , -halfSize. y , halfSize. z );
        corners [ 4 ] = position + rotation * new Vector3 ( -halfSize. x , halfSize. y , -halfSize. z );
        corners [ 5 ] = position + rotation * new Vector3 ( halfSize. x , halfSize. y , -halfSize. z );
        corners [ 6 ] = position + rotation * new Vector3 ( halfSize. x , halfSize. y , halfSize. z );
        corners [ 7 ] = position + rotation * new Vector3 ( -halfSize. x , halfSize. y , halfSize. z );

        // Find the min and max points to create a bounding box
        Vector3 min = corners [ 0 ];
        Vector3 max = corners [ 0 ];
        foreach ( Vector3 corner in corners )
        {
            min = Vector3. Min ( min , corner );
            max = Vector3. Max ( max , corner );
        }

        return new Bounds ( ( min + max ) / 2f , max - min );
    }

    Bounds GetIntersection ( Bounds b1 , Bounds b2 )
    {
        Vector3 min = Vector3. Max ( b1. min , b2. min );
        Vector3 max = Vector3. Min ( b1. max , b2. max );
        return new Bounds ( ( min + max ) / 2f , max - min );
    }

}

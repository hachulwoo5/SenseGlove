using UnityEngine;
using System. Collections;
public class ResultCalculate : MonoBehaviour
{
    public GameObject[] MovCube; // 인스펙터에서 할당할 큐브 오브젝트
    public GameObject[] Targetcube; // 인스펙터에서 할당할 큐브 오브젝트
    public GameObject [ ] Resetcube; // 인스펙터에서 할당할 큐브 오브젝트


    public float per;

    void Start ( )
    {


        per = 0;


    }

    private void Update ( )
    {
        if ( Input. GetKeyDown ( KeyCode. F8 ) )
        {
            StartCoroutine ( CalCulatePer ());
        }
        if ( Input. GetKeyDown ( KeyCode. F9 ) )
        {
            
        }
    }

    IEnumerator CalCulatePer()
    {
        if ( Targetcube != null )
        {
            for ( int x = 0 ; x < MovCube. Length ; x++ )
            {
                MovCube [ x ]. GetComponent<TestMani> ( ). a = 0;
            }

            TargerBoxAddColliders ( 0 );
            TargerBoxAddColliders ( 1 );
            TargerBoxAddColliders ( 2 );
            TargerBoxAddColliders ( 3 );
            TargerBoxAddColliders ( 4 );
            TargerBoxAddColliders ( 5 );

            for ( int x = 0 ; x < MovCube. Length ; x++ )
            {
                Debug. Log ( MovCube [ x ]. GetComponent<TestMani> ( ). a );         
            }
           


            yield return new WaitForSeconds ( 1f );

            Destroy ( Targetcube [ 0 ] );
            Destroy ( Targetcube [ 1 ] );
            Destroy ( Targetcube [ 2 ] );
            Destroy ( Targetcube [ 3 ] );
            Destroy ( Targetcube [ 4 ] );
            Destroy ( Targetcube [ 5 ] );

            for ( int x = 0 ; x < MovCube. Length ; x++ )
            {
                Resetcube [ x ]. gameObject. SetActive ( true );
            }

            per =
            ( MovCube [ 0 ]. GetComponent<TestMani> ( ). a +
            MovCube [ 1 ]. GetComponent<TestMani> ( ). a +
            MovCube [ 2 ]. GetComponent<TestMani> ( ). a +
            MovCube [ 3 ]. GetComponent<TestMani> ( ). a +
            MovCube [ 4 ]. GetComponent<TestMani> ( ). a +
            MovCube [ 5 ]. GetComponent<TestMani> ( ). a ) / 60;

            Debug. Log ( "per= " + per + "%" );

            
        }
    }


    void TargerBoxAddColliders (int number )
    {
        BoxCollider baseCollider = Targetcube[ number ]. GetComponent<BoxCollider> ( );
        if ( baseCollider == null )
        {
            baseCollider = Targetcube [ number ]. AddComponent<BoxCollider> ( );
        }

        Vector3 cubeSize = baseCollider. size;
        int collidersPerAxis = 10; // 10x10x10 = 1000
        float colliderSize = cubeSize. x / collidersPerAxis; // 각 콜라이더의 크기

        int count = 0;
        for ( int x = 0 ; x < collidersPerAxis ; x++ )
        {
            for ( int y = 0 ; y < collidersPerAxis ; y++ )
            {
                for ( int z = 0 ; z < collidersPerAxis ; z++ )
                {
                    if ( count == 0 )
                    {
                        baseCollider. center = new Vector3 (
                            ( x + 0.5f ) * colliderSize - cubeSize. x / 2 ,
                            ( y + 0.5f ) * colliderSize - cubeSize. y / 2 ,
                            ( z + 0.5f ) * colliderSize - cubeSize. z / 2
                        );
                        baseCollider. size = new Vector3 ( colliderSize , colliderSize , colliderSize );
                        baseCollider. isTrigger = true;
                    }
                    else
                    {
                        BoxCollider newCollider = Targetcube [ number ]. AddComponent<BoxCollider> ( );
                        newCollider. center = new Vector3 (
                            ( x + 0.5f ) * colliderSize - cubeSize. x / 2 ,
                            ( y + 0.5f ) * colliderSize - cubeSize. y / 2 ,
                            ( z + 0.5f ) * colliderSize - cubeSize. z / 2
                        );
                        newCollider. size = new Vector3 ( colliderSize , colliderSize , colliderSize );
                        newCollider. isTrigger = true;
                    }
                    count++;
                }
            }
        }
    }
}

using UnityEngine;
using System. Collections. Generic;

public class TargetBox : MonoBehaviour
{
    public GameObject MoveCube; // 할당할 큐브

    public List<GameObject> overlappingObjects = new List<GameObject> ( );

    private void OnTriggerEnter ( Collider other )
    {
        if ( other. gameObject != gameObject && other. CompareTag ( "Obj" ) )
        {
            overlappingObjects. Add ( other. gameObject );
        }
    }

    private void OnTriggerExit ( Collider other )
    {
        if ( overlappingObjects. Contains ( other. gameObject ) )
        {
            overlappingObjects. Remove ( other. gameObject );
        }
    }

    private void Update ( )
    {
        if ( Input. GetKeyDown ( KeyCode. F8 ) )
        {
            if ( MoveCube == null )
            {
                AssignClosestMoveCube ( );
            }
        }
        
    }

    void AssignClosestMoveCube ( )
    {
        if ( overlappingObjects. Count == 0 ) return;

        Vector3 targetCenter = GetComponent<Collider> ( ). bounds. center;
        float closestDistance = Mathf. Infinity;
        GameObject closestObj = null;

        foreach ( var obj in overlappingObjects )
        {
            float distance = Vector3. Distance ( targetCenter , obj. GetComponent<Collider> ( ). bounds. center );
            if ( distance < closestDistance )
            {
                closestDistance = distance;
                closestObj = obj;
            }
        }

        if ( closestObj != null )
        {
            MoveCube = closestObj;
            Debug. Log ( "MoveCube 할당됨: " + MoveCube. name );
        }
    }
}

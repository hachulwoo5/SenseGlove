using UnityEngine;
using Valve. VR;

public class HandController : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction;

    private SteamVR_Behaviour_Pose pose;
    private FixedJoint joint;
    private GameObject currentObject;

    void Start ( )
    {
        pose = GetComponent<SteamVR_Behaviour_Pose> ( );
        joint = GetComponent<FixedJoint> ( );
    }

    void Update ( )
    {
        if ( grabAction. GetStateDown ( pose. inputSource ) )
        {
            if ( currentObject == null )
            {
                GrabObject ( );
            }
        }

        if ( grabAction. GetStateUp ( pose. inputSource ) )
        {
            if ( currentObject != null )
            {
                ReleaseObject ( );
            }
        }
    }

    void OnTriggerEnter ( Collider other )
    {
        if ( other. gameObject. CompareTag ( "Grabbable" ) )
        {
            currentObject = other. gameObject;
        }
    }

    void OnTriggerExit ( Collider other )
    {
        if ( other. gameObject == currentObject )
        {
            currentObject = null;
        }
    }

    void GrabObject ( )
    {
        if ( currentObject != null )
        {
            Rigidbody targetBody = currentObject. GetComponent<Rigidbody> ( );
            if ( targetBody != null )
            {
                joint. connectedBody = targetBody;
            }
        }
    }

    void ReleaseObject ( )
    {
        if ( joint. connectedBody != null )
        {
            joint. connectedBody = null;
        }
    }
}
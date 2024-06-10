using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InObjScene : MonoBehaviour
{
    public ButtonOnly buttonOnly;

    public AudioClip clip; // ����� ����� Ŭ��
    private AudioSource audioSource;
    private void Start ( )
    {

        audioSource = gameObject. AddComponent<AudioSource> ( );
        audioSource. clip = clip;


    }
    private void Update ( )
    {


        if ( Input. GetKeyDown ( KeyCode. F6 ) )
        {
            buttonOnly. Timer ( );
            audioSource. Play ( );
        }
        if ( Input. GetKeyDown ( KeyCode. F7 ) )
        {
            buttonOnly. Timer ( );
        }
    }
}


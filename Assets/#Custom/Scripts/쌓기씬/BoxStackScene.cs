using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStackScene : MonoBehaviour
{
    public ButtonOnly buttonOnly;
    public GameObject [ ] Box;
    private bool isStart;

    public AudioClip clip; // 재생할 오디오 클립
    private AudioSource audioSource;
    private void Start ( )
    {
        isStart = false;
       
            audioSource = gameObject. AddComponent<AudioSource> ( );
            audioSource. clip = clip;
        

    }
    private void Update ( )
    {
        if(!isStart)
        {
            if ( buttonOnly. timerRunning )
            {
                for ( int i = 0 ; i < Box. Length ; i++ )
                {
                    Box [ i ]. GetComponent<MeshRenderer> ( ). enabled = true;
                }
                isStart = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.F6))
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

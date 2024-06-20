using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class InObjScene : MonoBehaviour
{
    public ButtonOnly buttonOnly;
    public AudioClip clip; // 재생할 오디오 클립
    private AudioSource audioSource;

    public GameObject [ ] shapes; // 도형 배열
    private Vector3 [ ] initialPositions; // 도형들의 초기 위치를 저장할 배열

    private void Start ( )
    {
        audioSource = gameObject. AddComponent<AudioSource> ( );
        audioSource. clip = clip;

        // 초기 위치 배열 초기화 및 도형들의 초기 위치 저장
        initialPositions = new Vector3 [ shapes. Length ];
        for ( int i = 0 ; i < shapes. Length ; i++ )
        {
            initialPositions [ i ] = shapes [ i ]. transform. position;
        }
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

        // 키패드 1, 2, 3, 4, 5를 눌렀을 때 각각의 도형을 초기 위치로 이동
        if ( Input. GetKeyDown ( KeyCode. Keypad1 ) )
        {
            shapes [ 0 ]. transform. position = initialPositions [ 0 ];
        }
        if ( Input. GetKeyDown ( KeyCode. Keypad2 ) )
        {
            shapes [ 1 ]. transform. position = initialPositions [ 1 ];
        }
        if ( Input. GetKeyDown ( KeyCode. Keypad3 ) )
        {
            shapes [ 2 ]. transform. position = initialPositions [ 2 ];
        }
        if ( Input. GetKeyDown ( KeyCode. Keypad4 ) )
        {
            shapes [ 3 ]. transform. position = initialPositions [ 3 ];
        }
        if ( Input. GetKeyDown ( KeyCode. Keypad5 ) )
        {
            shapes [ 4 ]. transform. position = initialPositions [ 4 ];
        }
    }
}

using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class InObjScene : MonoBehaviour
{
    public ButtonOnly buttonOnly;
    public AudioClip clip; // ����� ����� Ŭ��
    private AudioSource audioSource;

    public GameObject [ ] shapes; // ���� �迭
    private Vector3 [ ] initialPositions; // �������� �ʱ� ��ġ�� ������ �迭

    private void Start ( )
    {
        audioSource = gameObject. AddComponent<AudioSource> ( );
        audioSource. clip = clip;

        // �ʱ� ��ġ �迭 �ʱ�ȭ �� �������� �ʱ� ��ġ ����
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

        // Ű�е� 1, 2, 3, 4, 5�� ������ �� ������ ������ �ʱ� ��ġ�� �̵�
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

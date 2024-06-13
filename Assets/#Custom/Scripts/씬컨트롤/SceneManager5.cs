using System. Collections;
using System. Collections. Generic;
using UnityEngine;
using UnityEngine. SceneManagement; // �� ��ȯ�� ���� �ʿ��� ���ӽ����̽�

public class SceneManager5 : MonoBehaviour
{
    // �� �̸��� �迭�� ����
    private string [ ] sceneNames = new string [ ]
    {
        "#VR Test CubeStack",
        "#VR Test InObj",
        "#VR Test Throw",
        "#VR Test Vive CubeStack",
        "#VR Test Vive InObj",
        "#VR Test Vive Throw"
    };

    // Update is called once per frame
    void Update ( )
    {
        // Ű�е� ���� Ű �Է��� �����Ͽ� �ش� ������ ��ȯ
        if ( Input. GetKeyDown ( KeyCode. Keypad1 ) )
        {
            LoadScene ( 0 );
        }
        else if ( Input. GetKeyDown ( KeyCode. Keypad2 ) )
        {
            LoadScene ( 1 );
        }
        else if ( Input. GetKeyDown ( KeyCode. Keypad3 ) )
        {
            LoadScene ( 2 );
        }
        else if ( Input. GetKeyDown ( KeyCode. Keypad4 ) )
        {
            LoadScene ( 3 );
        }
        else if ( Input. GetKeyDown ( KeyCode. Keypad5 ) )
        {
            LoadScene ( 4 );
        }
        else if ( Input. GetKeyDown ( KeyCode. Keypad6 ) )
        {
            LoadScene ( 5 );
        }
    }

    // �ε����� �ش��ϴ� ���� �ε�
    void LoadScene ( int index )
    {
        if ( index >= 0 && index < sceneNames. Length )
        {
            SceneManager. LoadScene ( sceneNames [ index ] );
        }
    }
}

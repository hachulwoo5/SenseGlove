using System. Collections;
using System. Collections. Generic;
using UnityEngine;
using UnityEngine. SceneManagement; // 씬 전환을 위해 필요한 네임스페이스

public class SceneManager5 : MonoBehaviour
{
    // 씬 이름을 배열로 저장
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
        // 키패드 숫자 키 입력을 감지하여 해당 씬으로 전환
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

    // 인덱스에 해당하는 씬을 로드
    void LoadScene ( int index )
    {
        if ( index >= 0 && index < sceneNames. Length )
        {
            SceneManager. LoadScene ( sceneNames [ index ] );
        }
    }
}

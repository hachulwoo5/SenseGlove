using System. Collections;
using System. Collections. Generic;
using System. IO;
using System. Text;
using UnityEngine;

public class DataSaved_InObj : MonoBehaviour
{
    public string namer;
    public float elapsedTime;

    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {
        elapsedTime = GameObject. Find ( "Coll" ). GetComponent<ButtonOnly> ( ). elapsedTime;
        namer = GameObject. Find ( "참가자 이름" ). GetComponent<Name> ( ). name;

        if ( Input. GetKeyDown ( KeyCode. F12 ) )
        {
            SaveDataToCSV ( namer , elapsedTime  );
        }
    }
    public void SaveData()
    {
        SaveDataToCSV ( namer , elapsedTime );
    }
    void SaveDataToCSV ( string name , float elapsedTime   )
    {
        string sceneName = UnityEngine. SceneManagement. SceneManager. GetActiveScene ( ). name; // 현재 씬 이름 가져오기
        string path = $"{sceneName}_data.csv"; // 파일 경로 지정

        // 현재 시간 가져오기 (시간:분:초 형식)
        string currentTime = System. DateTime. Now. ToString ( "yyyy-MM-dd HH:mm:ss" ); // 초까지 포함

        // CSV 형식의 문자열 포맷
        string csvLine = $"{name},{currentTime},{elapsedTime:F2}\n";

        // 파일 스트림을 사용하여 UTF-8 인코딩으로 파일에 데이터 추가
        try
        {
            using ( FileStream fs = new FileStream ( path , FileMode. Append , FileAccess. Write , FileShare. None ) )
            {
                using ( StreamWriter writer = new StreamWriter ( fs , Encoding. UTF8 ) )
                {
                    writer. Write ( csvLine );
                }
            }
            Debug. Log ( "데이터가 파일에 저장되었습니다: " + path );
        }
        catch ( IOException ex )
        {
            Debug. LogError ( "파일 저장 중 오류가 발생했습니다: " + ex. Message );
        }
    }
}

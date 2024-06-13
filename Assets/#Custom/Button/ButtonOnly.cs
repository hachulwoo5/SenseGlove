using System. Collections;
using System. Collections. Generic;
using UnityEngine;
using UnityEngine. Events;
using TMPro;
using System. IO;
using UnityEngine. SceneManagement;

public class ButtonOnly : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public GameObject presser;
    AudioSource sound;
    public bool isPressed;

    public bool timerRunning;
    public float elapsedTime;
    public TMP_Text elapsedTimeText;

    public GameObject [ ] ResetObjectList;
    private Vector3 [ ] initialPositions; // 초기 위치를 저장할 변수

    public MeshRenderer Base;
    public MeshRenderer Press;

    public string Name; // 인스펙터에서 기입할 이름 변수

    void Start ( )
    {
        sound = GetComponent<AudioSource> ( );
        isPressed = false;
        timerRunning = false;
        elapsedTime = 0f;
        Name = GameObject. Find ( "참가자 이름" ). GetComponent<Name> ( ). name;
        if ( elapsedTimeText != null )
        {
            
        }

        initialPositions = new Vector3 [ ResetObjectList. Length ];
        for ( int i = 0 ; i < ResetObjectList. Length ; i++ )
        {
            initialPositions [ i ] = ResetObjectList [ i ]. transform. position;
        }
    }

    private void OnTriggerEnter ( Collider other )
    {
        if ( !isPressed )
        {
            Debug. Log ( "눌림" );
            button. transform. position -= new Vector3 ( 0 , 0.01f , 0 );
            presser = other. gameObject;
            onPress. Invoke ( );
            sound. Play ( );
            isPressed = true;
        }
    }

    private void OnTriggerExit ( Collider other )
    {
        if ( other. gameObject == presser )
        {
            button. transform. position += new Vector3 ( 0 , 0.01f , 0 );
            onRelease. Invoke ( );
            isPressed = false;
            presser = null;
            this. gameObject. GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void Timer ( )
    {
        if ( !timerRunning )
        {
            // 타이머 시작
            Debug. Log ( "타이머 시작" );
            timerRunning = true;
            StartCoroutine ( TimerCoroutine ( ) );
            Base. GetComponent<MeshRenderer> ( ). enabled = false;
            Press. GetComponent<MeshRenderer> ( ). enabled = false;
            
        }
        else
        {
            // 타이머 중지
            Debug. Log ( "타이머 중지" );
            timerRunning = false;
            StopCoroutine ( TimerCoroutine ( ) );
            Debug. Log ( "경과 시간: " + elapsedTime );

            // Save the elapsed time to a file
            SaveElapsedTimeToFile ( elapsedTime );
        }
    }
    private IEnumerator Delay ( )
    {
        yield return new WaitForSeconds(0.12f);
        this. gameObject. SetActive ( false );
    }
    private IEnumerator TimerCoroutine ( )
    {
        elapsedTime = 0f;
        while ( timerRunning )
        {
            elapsedTime += Time. deltaTime;
            if ( elapsedTimeText != null )
            {
            }
            yield return null;
        }
    }

    public void ObjReset ( )
    {
        for ( int i = 0 ; i < ResetObjectList. Length ; i++ )
        {
            ResetObjectList [ i ]. transform. position = initialPositions [ i ];
        }
    }

    private void SaveElapsedTimeToFile ( float time )
    {
        string sceneName = SceneManager. GetActiveScene ( ). name; // 현재 씬 이름 가져오기
        string path = $"{sceneName}.txt"; // 파일 경로 지정

        // 현재 시간 가져오기 (시간:분:초 형식)
        string currentTime = System. DateTime. Now. ToString ( "yyyy-MM-dd HH:mm:ss" );

        // 저장할 문자열 포맷
        string elapsedTimeString = $"이름: {Name}, 기록된 시간: {currentTime}, 경과 시간: {time:F2}s\n";

        // 파일에 경과 시간 추가
        File. AppendAllText ( path , elapsedTimeString );

        Debug. Log ( "경과 시간이 파일에 저장되었습니다: " + path );
    }
}

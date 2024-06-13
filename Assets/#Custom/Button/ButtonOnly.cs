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
    private Vector3 [ ] initialPositions; // �ʱ� ��ġ�� ������ ����

    public MeshRenderer Base;
    public MeshRenderer Press;

    public string Name; // �ν����Ϳ��� ������ �̸� ����

    void Start ( )
    {
        sound = GetComponent<AudioSource> ( );
        isPressed = false;
        timerRunning = false;
        elapsedTime = 0f;
        Name = GameObject. Find ( "������ �̸�" ). GetComponent<Name> ( ). name;
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
            Debug. Log ( "����" );
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
            // Ÿ�̸� ����
            Debug. Log ( "Ÿ�̸� ����" );
            timerRunning = true;
            StartCoroutine ( TimerCoroutine ( ) );
            Base. GetComponent<MeshRenderer> ( ). enabled = false;
            Press. GetComponent<MeshRenderer> ( ). enabled = false;
            
        }
        else
        {
            // Ÿ�̸� ����
            Debug. Log ( "Ÿ�̸� ����" );
            timerRunning = false;
            StopCoroutine ( TimerCoroutine ( ) );
            Debug. Log ( "��� �ð�: " + elapsedTime );

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
        string sceneName = SceneManager. GetActiveScene ( ). name; // ���� �� �̸� ��������
        string path = $"{sceneName}.txt"; // ���� ��� ����

        // ���� �ð� �������� (�ð�:��:�� ����)
        string currentTime = System. DateTime. Now. ToString ( "yyyy-MM-dd HH:mm:ss" );

        // ������ ���ڿ� ����
        string elapsedTimeString = $"�̸�: {Name}, ��ϵ� �ð�: {currentTime}, ��� �ð�: {time:F2}s\n";

        // ���Ͽ� ��� �ð� �߰�
        File. AppendAllText ( path , elapsedTimeString );

        Debug. Log ( "��� �ð��� ���Ͽ� ����Ǿ����ϴ�: " + path );
    }
}

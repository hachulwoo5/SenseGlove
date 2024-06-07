using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System. Collections;
using TMPro;

public class ButtonOnly : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public GameObject presser;
    AudioSource sound;
    public bool isPressed;


    private bool timerRunning;
    public float elapsedTime;
    public TMP_Text elapsedTimeText;

    public GameObject [ ] ResetObjectList;
    private Vector3[] initialPositions; // �ʱ� ��ġ�� ������ ����

    void Start ( )
    {
        sound = GetComponent<AudioSource> ( );
        isPressed = false;
        timerRunning = false;
        elapsedTime = 0f;

        if ( elapsedTimeText != null )
        {
            elapsedTimeText. text = "Elapsed Time: 0.00s";
        }

        initialPositions = new Vector3 [ ResetObjectList. Length ];
        for ( int i=0 ;i< ResetObjectList.Length ;i++ )
        {
            initialPositions[i] = ResetObjectList[i]. transform. position;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            Debug. Log ( "����" );
            button. transform.position -= new Vector3(0, 0.01f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.position += new Vector3(0, 0.01f, 0);
            onRelease.Invoke();
            isPressed = false;
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
        }
        else
        {
            // Ÿ�̸� ����
            Debug. Log ( "Ÿ�̸� ����" );
            timerRunning = false;
            StopCoroutine ( TimerCoroutine ( ) );
            Debug. Log ( "��� �ð�: " + elapsedTime );
        }
    }

    private IEnumerator TimerCoroutine ( )
    {
        elapsedTime = 0f;
        while ( timerRunning )
        {
            elapsedTime += Time. deltaTime;
            if ( elapsedTimeText != null )
            {
                elapsedTimeText. text = $"Elapsed Time: {elapsedTime:F2}s";
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
}

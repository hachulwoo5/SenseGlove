using System. Collections;
using System. Collections. Generic;
using System. IO;
using System. Text;
using UnityEngine;

public class DataSaved : MonoBehaviour
{
    public string namer;
    public float distanceValue;
    public float elapsedTime;

    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {
        elapsedTime = GameObject. Find ( "Coll" ). GetComponent<ButtonOnly> ( ). elapsedTime;
        distanceValue = GameObject. Find ( "�� ����" ). GetComponent<BoxStackCalculate> ( ). totalDifference;
        namer = GameObject. Find ( "������ �̸�" ). GetComponent<Name> ( ). name;
        if ( Input. GetKeyDown ( KeyCode. F12 ) )
        {
            SaveDataToCSV ( namer , elapsedTime , distanceValue );
        }
    }

    void SaveDataToCSV ( string name , float elapsedTime , float distance )
    {
        string sceneName = UnityEngine. SceneManagement. SceneManager. GetActiveScene ( ). name; // ���� �� �̸� ��������
        string path = $"{sceneName}_data.csv"; // ���� ��� ����

        // ���� �ð� �������� (�ð�:��:�� ����)
        string currentTime = System. DateTime. Now. ToString ( "yyyy-MM-dd HH:mm:ss" ); // �ʱ��� ����

        // CSV ������ ���ڿ� ����
        string csvLine = $"{name},{currentTime},{elapsedTime:F2},{distance:F2}\n";

        // ���� ��Ʈ���� ����Ͽ� UTF-8 ���ڵ����� ���Ͽ� ������ �߰�
        try
        {
            using ( FileStream fs = new FileStream ( path , FileMode. Append , FileAccess. Write , FileShare. None ) )
            {
                using ( StreamWriter writer = new StreamWriter ( fs , Encoding. UTF8 ) )
                {
                    writer. Write ( csvLine );
                }
            }
            Debug. Log ( "�����Ͱ� ���Ͽ� ����Ǿ����ϴ�: " + path );
        }
        catch ( IOException ex )
        {
            Debug. LogError ( "���� ���� �� ������ �߻��߽��ϴ�: " + ex. Message );
        }
    }
}

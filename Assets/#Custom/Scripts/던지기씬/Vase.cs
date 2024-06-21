using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public AudioClip collisionClip; // �浹 �� ����� ����� Ŭ��
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start ( )
    {
        // AudioSource ������Ʈ�� �߰��ϰ� ����� Ŭ���� �����մϴ�.
        audioSource = gameObject. AddComponent<AudioSource> ( );
        audioSource. clip = collisionClip;
    }

    // �浹�� �߻����� �� ȣ��Ǵ� �޼���
    private void OnCollisionEnter ( Collision collision )
    {
        // �浹�� ������Ʈ�� �±װ� "Obj"���� Ȯ���մϴ�.
        if ( collision. gameObject. tag == "Obj" )
        {
            // ������� ���� ��� ������ ���� ���� ����մϴ�.
            if ( !audioSource. isPlaying )
            {
                audioSource. Play ( );
            }
        }
    }

    // Update is called once per frame
    void Update ( )
    {
        // �ʿ信 ���� ������Ʈ ������ �߰��մϴ�.
    }
}

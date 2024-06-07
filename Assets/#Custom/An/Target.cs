using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start ( )
    {
        // Animator ������Ʈ�� �����ɴϴ�.
        animator = GetComponent<Animator> ( );
    }

    // Update is called once per frame
    void Update ( )
    {

    }

    // OnTriggerEnter�� �ٸ� �ݶ��̴��� �� ������Ʈ�� Ʈ���� �ݶ��̴��� �浹�� �� ȣ��˴ϴ�.
    private void OnTriggerEnter ( Collider other )
    {
        // �浹�� ������Ʈ�� �±װ� "Obj"���� Ȯ���մϴ�.
        if ( other. CompareTag ( "Obj" ) )
        {
            // �ִϸ��̼� Ʈ���Ÿ� �����Ͽ� �ִϸ��̼��� ����մϴ�.
            animator. SetTrigger ( "PlayAnimation" );
        }
    }
}

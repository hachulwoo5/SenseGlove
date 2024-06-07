using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start ( )
    {
        // Animator 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator> ( );
    }

    // Update is called once per frame
    void Update ( )
    {

    }

    // OnTriggerEnter는 다른 콜라이더가 이 오브젝트의 트리거 콜라이더와 충돌할 때 호출됩니다.
    private void OnTriggerEnter ( Collider other )
    {
        // 충돌한 오브젝트의 태그가 "Obj"인지 확인합니다.
        if ( other. CompareTag ( "Obj" ) )
        {
            // 애니메이션 트리거를 설정하여 애니메이션을 재생합니다.
            animator. SetTrigger ( "PlayAnimation" );
        }
    }
}

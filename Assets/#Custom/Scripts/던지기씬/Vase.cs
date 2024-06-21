using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public AudioClip collisionClip; // 충돌 시 재생할 오디오 클립
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start ( )
    {
        // AudioSource 컴포넌트를 추가하고 오디오 클립을 설정합니다.
        audioSource = gameObject. AddComponent<AudioSource> ( );
        audioSource. clip = collisionClip;
    }

    // 충돌이 발생했을 때 호출되는 메서드
    private void OnCollisionEnter ( Collision collision )
    {
        // 충돌한 오브젝트의 태그가 "Obj"인지 확인합니다.
        if ( collision. gameObject. tag == "Obj" )
        {
            // 오디오가 아직 재생 중이지 않을 때만 재생합니다.
            if ( !audioSource. isPlaying )
            {
                audioSource. Play ( );
            }
        }
    }

    // Update is called once per frame
    void Update ( )
    {
        // 필요에 따라 업데이트 로직을 추가합니다.
    }
}

using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class ThrowScene : MonoBehaviour
{
    public AudioClip clip; // 재생할 오디오 클립
    private AudioSource audioSource;
    public int Number;

    public ButtonOnly buttonOnly;
    // Start is called before the first frame update
    void Start ( )
    {
        audioSource = gameObject. AddComponent<AudioSource> ( );
        audioSource. clip = clip;
        Number = 0;
    }

 

    private void OnTriggerEnter ( Collider other )
    {
        if ( other. gameObject. name == "Vase" )
        {
            Number++;

            // 오디오 클립 재생

            // Vase 오브젝트 제거

            if(Number==3)
            {
                buttonOnly. Timer ( );
                audioSource. Play ( );
            }
        }
    }

    private void Update ( )
    {
     

        if ( Input. GetKeyDown ( KeyCode. F6 ) )
        {
            buttonOnly. Timer ( );
            audioSource. Play ( );
        }
        if ( Input. GetKeyDown ( KeyCode. F7 ) )
        {
            buttonOnly. Timer ( );
        }
    }
}

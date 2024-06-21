using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class BoxStackScene : MonoBehaviour
{
    public ButtonOnly buttonOnly;

    public AudioClip clip; // ����� ����� Ŭ��
    private AudioSource audioSource;

    public GameObject [ ] Targetboxes;
    public Material [ ] TargetboxMat;

    private void Start ( )
    {
        audioSource = gameObject. AddComponent<AudioSource> ( );
        audioSource. clip = clip;

        
    }

    private void Update ( )
    {
        if ( Input. GetKeyDown ( KeyCode. F6 ) )
        {
            buttonOnly. Timer ( );
        }
        if ( Input. GetKeyDown ( KeyCode. F7 ) )
        {
            buttonOnly. Timer ( );
            audioSource. Play ( );
        }
    }

    public void StartButton ( )
    {
        // StartButton ���� ������ �ʿ��� ��� ���⿡ �߰�
        // �����ϰ� ���� ���͸��� �迭 ����
        Material [ ] shuffledMaterials = ShuffleMaterials ( TargetboxMat );

        // Ÿ�� �ڽ��鿡 �ߺ����� �ʰ� ���͸��� �Ҵ�
        for ( int i = 0 ; i < Targetboxes. Length ; i++ )
        {
            if ( Targetboxes [ i ] != null && i < shuffledMaterials. Length )
            {
                Renderer renderer = Targetboxes [ i ]. GetComponent<Renderer> ( );
                MeshRenderer mrenderer = Targetboxes [ i ]. GetComponent<MeshRenderer> ( );

                if ( renderer != null )
                {
                    mrenderer. enabled = true;
                    renderer. material = shuffledMaterials [ i ];
                }
            }
        }
    }

    private Material [ ] ShuffleMaterials ( Material [ ] materials )
    {
        Material [ ] shuffled = ( Material [ ] ) materials. Clone ( );
        for ( int i = 0 ; i < shuffled. Length ; i++ )
        {
            int rnd = Random. Range ( i , shuffled. Length );
            Material temp = shuffled [ i ];
            shuffled [ i ] = shuffled [ rnd ];
            shuffled [ rnd ] = temp;
        }
        return shuffled;
    }
}

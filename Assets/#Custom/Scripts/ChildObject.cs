using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObject : MonoBehaviour
{
    // �̺�Ʈ ����
    public delegate void ColorChangedHandler(Color newColor);
    public event ColorChangedHandler OnColorChanged;

    // �ڽ� ������Ʈ�� ���� ���� �Լ�
    public void ChangeColor(Color newColor )
    {
        // ���� ���� ����...
        Renderer sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            sphereRenderer. material.color = newColor;
        }
        // �̺�Ʈ ȣ��
        if (OnColorChanged != null)
        {
            OnColorChanged(newColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obj")
        {
            ChangeColor(new Color(0f,1f,0f,1f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Obj")
        {
            ChangeColor(new Color(1f, 0f, 0f, 1f));

           // ChangeColor ( new Color ( 1f , 0f , 0f , 0f ) );
        }
    }

}
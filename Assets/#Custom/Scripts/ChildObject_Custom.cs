using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObject_Custom : MonoBehaviour
{
    // �̺�Ʈ ����
    public delegate void ColorChangedHandler(Color newColor, GameObject grabbedObj);
    public event ColorChangedHandler OnColorChanged;

    public List<GameObject> ObjList;

    public GameObject grabbedObj;
    public bool isExit;
    public bool isGrabbed;
    // �ڽ� ������Ʈ�� ���� ���� �Լ�
    public void ChangeColor(Color newColor)
    {

        // ���� ���� ����...
        Renderer sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = newColor;
        }
        // �̺�Ʈ ȣ��
        if (OnColorChanged != null)
        {
            OnColorChanged(newColor, grabbedObj);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        isExit = false;
        grabbedObj = other.gameObject;
        if (other.gameObject.tag == "Obj")
        {
            ChangeColor(new Color(0f, 1f, 0f, 1f));
            ObjList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isExit = true;
        grabbedObj = null;

        if (other.gameObject.tag == "Obj")
        {
            ChangeColor(new Color(1f, 0f, 0f, 1f));
            ObjList.Remove(other.gameObject);
            // ChangeColor ( new Color ( 1f , 0f , 0f , 0f ) );
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObject_Custom : MonoBehaviour
{
    // 이벤트 정의
    public delegate void ColorChangedHandler(Color newColor, GameObject grabbedObj);
    public event ColorChangedHandler OnColorChanged;

    public List<GameObject> ObjList;

    public GameObject grabbedObj;
    public bool isExit;
    public bool isGrabbed;
    // 자식 오브젝트의 색상 변경 함수
    public void ChangeColor(Color newColor)
    {

        // 색상 변경 로직...
        Renderer sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = newColor;
        }
        // 이벤트 호출
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
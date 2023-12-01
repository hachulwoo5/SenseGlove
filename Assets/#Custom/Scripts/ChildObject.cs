using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObject : MonoBehaviour
{
    // 이벤트 정의
    public delegate void ColorChangedHandler(Color newColor);
    public event ColorChangedHandler OnColorChanged;

    // 자식 오브젝트의 색상 변경 함수
    public void ChangeColor(Color newColor )
    {
        // 색상 변경 로직...
        Renderer sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            sphereRenderer. material.color = newColor;
        }
        // 이벤트 호출
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
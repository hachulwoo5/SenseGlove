using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObject_VR : MonoBehaviour
{ // 이벤트 정의
    public delegate void ColorChangedHandler ( Color newColor , float ObjectMass , GameObject grabbedObj );
    public event ColorChangedHandler OnColorChanged;
    public delegate void SideChangedHandler ( bool isSideGrab );
    public event SideChangedHandler sideChangedHandler;
    Renderer sphereRenderer;
    private Color originalColor; // 원래 색상을 저장하는 변수
    public bool isSidePointGrab;

    public float objMass;
    public GameObject grabbedObj;

    // 자식 오브젝트의 색상 변경 함수
    public void ChangeColor ( Color newColor , float objmass )
    {
        // 색상 변경 로직...
        Renderer sphereRenderer = GetComponent<Renderer> ( );
        if ( sphereRenderer != null )
        {
            sphereRenderer. material. color = newColor;
        }
        // 이벤트 호출
        if ( OnColorChanged != null )
        {
            OnColorChanged ( newColor , objmass ,grabbedObj );
        }
    }

    // 초기화 함수
    void Start ( )
    {
        // 초기에 원래 색상 저장
        objMass = 0;
        sphereRenderer = GetComponent<Renderer> ( );
        if ( sphereRenderer != null )
        {
            originalColor = sphereRenderer. material. color;
        }
    }
    void Update ( )
    {
        // Toggle MeshRenderer on and off when the "u" key is pressed
        if ( Input. GetKeyDown ( KeyCode. U ) )
        {
            if ( sphereRenderer != null )
            {
                sphereRenderer. enabled = !sphereRenderer. enabled;
            }
        }
    }
    private void OnTriggerEnter ( Collider other )
    {
        if ( other. gameObject. tag == "Obj" )
        {
            // 녹색으로 색상 변경
            objMass = other. GetComponent<Rigidbody> ( ). mass;
            grabbedObj = other. gameObject;
            ChangeColor ( new Color ( 0f , 1f , 0f , 1f ) , objMass );
            if ( this. gameObject. tag == "SideSphere" )
            {
                isSidePointGrab = true;
                sideChangedHandler ( isSidePointGrab );
            }
        }
    }

    private void OnTriggerExit ( Collider other )
    {
        if ( other. gameObject. tag == "Obj" )
        {
            // 빨간색으로 색상 변경
            objMass = 0;
            grabbedObj =null;
            ChangeColor ( originalColor , objMass );
            if ( this. gameObject. tag == "SideSphere" )
            {
                isSidePointGrab = false;
                sideChangedHandler ( isSidePointGrab );
            }
        }
    }

    // 추가된 부분: 원래 색상으로 복원하는 함수
    public void RestoreOriginalColor ( )
    {
        ChangeColor ( originalColor , objMass );
    }
}
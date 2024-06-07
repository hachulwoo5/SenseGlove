using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObject : MonoBehaviour
{
    // �̺�Ʈ ����
    public delegate void ColorChangedHandler(Color newColor,float ObjectMass);
    public event ColorChangedHandler OnColorChanged;
    public delegate void SideChangedHandler(bool isSideGrab);
    public event SideChangedHandler sideChangedHandler;
    Renderer sphereRenderer;
    private Color originalColor; // ���� ������ �����ϴ� ����
    public bool isSidePointGrab;

    public float objMass;
    // �ڽ� ������Ʈ�� ���� ���� �Լ�
    public void ChangeColor(Color newColor,float objmass)
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
            OnColorChanged(newColor,objmass);
        }
    }

    // �ʱ�ȭ �Լ�
    void Start()
    {
        // �ʱ⿡ ���� ���� ����
        objMass = 0;
        sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            originalColor = sphereRenderer.material.color;
        }

        if ( sphereRenderer != null )
        {
            sphereRenderer. enabled = !sphereRenderer. enabled;
        }
    }
    void Update()
    {
        // Toggle MeshRenderer on and off when the "u" key is pressed
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (sphereRenderer != null)
            {
                sphereRenderer.enabled = !sphereRenderer.enabled;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obj")
        {
            // ������� ���� ����
            objMass = other.GetComponent<Rigidbody>().mass;
            ChangeColor(new Color(0f, 1f, 0f, 1f), objMass);
            if (this.gameObject.tag == "SideSphere")
            {
                isSidePointGrab = true;
                sideChangedHandler(isSidePointGrab);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Obj")
        {
            // ���������� ���� ����
            objMass = 0;
            ChangeColor(originalColor, objMass);
            if (this.gameObject.tag == "SideSphere")
            {
                isSidePointGrab = false;
                sideChangedHandler(isSidePointGrab);
            }
        }
    }

    // �߰��� �κ�: ���� �������� �����ϴ� �Լ�
    public void RestoreOriginalColor()
    {
        ChangeColor(originalColor,objMass);
    }
}
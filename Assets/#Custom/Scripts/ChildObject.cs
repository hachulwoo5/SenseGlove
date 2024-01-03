using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObject : MonoBehaviour
{
    // �̺�Ʈ ����
    public delegate void ColorChangedHandler(Color newColor);
    public event ColorChangedHandler OnColorChanged;
    public delegate void SideChangedHandler(bool isSideGrab);
    public event SideChangedHandler sideChangedHandler;
    Renderer sphereRenderer;
    private Color originalColor; // ���� ������ �����ϴ� ����
    public bool isSidePointGrab;

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
            OnColorChanged(newColor);
        }
    }

    // �ʱ�ȭ �Լ�
    void Start()
    {
        // �ʱ⿡ ���� ���� ����
        sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            originalColor = sphereRenderer.material.color;
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
            ChangeColor(new Color(0f, 1f, 0f, 1f));

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
            ChangeColor(originalColor);

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
        ChangeColor(originalColor);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObject : MonoBehaviour
{
    public float beingTouched = 0;
    public float touchPercentage;
    public bool isReadyGrab;
    ChildObject[] childScripts;

    public bool isSideGrab;

    private void Start()
    {
        // �θ� ������Ʈ �Ʒ��� �ִ� ��� �ڽ� ������Ʈ�� ChildObject ��ũ��Ʈ�� ������
        childScripts = GetComponentsInChildren<ChildObject>();

        // �� �ڽ� ������Ʈ�� ���� �̺�Ʈ �ڵ鷯 ���
        foreach (ChildObject childScript in childScripts)
        {
            // �� �ڽ� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִ� ��쿡�� �̺�Ʈ �ڵ鷯 ���
            // �ؿ��� �� ���ؼ� �̺�Ʈ �Ѱܿ��� HandleColorChanged�� �ߵ��ع���
            // ���⼭ ��������
            if (childScript.gameObject.activeSelf)
            {
                childScript.OnColorChanged += HandleColorChanged;
                childScript.sideChangedHandler += SidePointChanged;
             }
        }

        InvokeRepeating("LogBeingTouchedRatio", 0f, 1.2f);

    }
    private void LogBeingTouchedRatio()
    {
        // �α� ���
       // Debug.Log(this.transform.name +"��ġ ���� ���� = " + (beingTouched / childScripts.Length) * 100f + " % ");
    }

    private void HandleColorChanged(Color newColor)
    {
        Color targetColor1 = Color.red;
        Color targetColor2 = new Color(0x60 / 255f, 0 / 255f, 0xFF / 255f, 1f);

        // ������ �ٲ� �ڽ� ������Ʈ�� ���� ����
        if (newColor == Color.green)
        {
            beingTouched++;
        }
        if (newColor == targetColor1 || newColor == targetColor2)
        {
            beingTouched--;
        }

        touchPercentage = beingTouched / childScripts. Length;
        if(beingTouched>=1)
        {
            isReadyGrab = true;
        }
        else
        {
            isReadyGrab = false;
        }
    }

    // ���ϵ忡�� ���̵尡 �������� ���θ� ������ �� bool(isSideGrab)�� �����Ų��
    private void SidePointChanged(bool isChildSide)
    {
        isSideGrab = isChildSide;

    }


}
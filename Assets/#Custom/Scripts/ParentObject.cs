using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObject : MonoBehaviour
{
    public float beingTouched = 0;
    public float touchPercentage;
    ChildObject[] childScripts;

    private void Start()
    {
        // �θ� ������Ʈ �Ʒ��� �ִ� ��� �ڽ� ������Ʈ�� ChildObject ��ũ��Ʈ�� ������
        childScripts = GetComponentsInChildren<ChildObject>();

        // �� �ڽ� ������Ʈ�� ���� �̺�Ʈ �ڵ鷯 ���
        foreach (ChildObject childScript in childScripts)
        {
            // �� �ڽ� ������Ʈ�� �̺�Ʈ �ڵ鷯 ���
            childScript.OnColorChanged += HandleColorChanged;
        }

        InvokeRepeating("LogBeingTouchedRatio", 0f, 1.2f);

    }
    private void LogBeingTouchedRatio()
    {
        // �α� ���
        Debug.Log(this.transform.name +"��ġ ���� ���� = " + (beingTouched / childScripts.Length) * 100f + " % ");
    }

    private void HandleColorChanged(Color newColor)
    {

        // ������ �ٲ� �ڽ� ������Ʈ�� ���� ����
        if (newColor == Color.green)
        {
            beingTouched++;
        }
        if (newColor == Color.red)
        {
            beingTouched--;
        }

        touchPercentage = beingTouched / childScripts. Length;
    }


}
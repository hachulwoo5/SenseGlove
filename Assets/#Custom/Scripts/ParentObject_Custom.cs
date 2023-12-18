using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObject_Custom : MonoBehaviour
{
    public float beingTouched = 0;
    public float touchPercentage;
    public bool isGrabbing;
    public List<ChildObject_Custom> childScripts = new List<ChildObject_Custom>();

    public GameObject grabbedObject;
    public List<GameObject> ObjList;
    public Transform handTransform;
    private void Start()
    {
        // �θ� ������Ʈ �Ʒ��� �ִ� ��� �ڽ� ������Ʈ�� ChildObject ��ũ��Ʈ�� ������
        childScripts.AddRange(GetComponentsInChildren<ChildObject_Custom>());

        // �� �ڽ� ������Ʈ�� ���� �̺�Ʈ �ڵ鷯 ���
        foreach (ChildObject_Custom childScript in childScripts)
        {
            // �� �ڽ� ������Ʈ�� �̺�Ʈ �ڵ鷯 ���
            childScript.OnColorChanged += HandleColorChanged;

        }

        InvokeRepeating("LogBeingTouchedRatio", 0f, 1.2f);

    }
    private void LogBeingTouchedRatio()
    {
        // �α� ���
        //  Debug.Log(this.transform.name +"��ġ ���� ���� = " + (beingTouched / childScripts.Count) * 100f + " % ");
    }

    public void HandleColorChanged(Color newColor, GameObject other)
    {
        if (newColor == Color.green)
        {
            beingTouched++;
        }
        if (newColor == Color.red)
        {
            beingTouched--;
        }

        touchPercentage = beingTouched / childScripts.Count;

        // ��� �հ����� �˻��� ������ ������Ʈ�� �� ������ �˻���
        // ������ ������ ����ؼ� 2�� �������� null�� �ƴ� ������Ʈ�� �˻��ϰ� ��
        // �ᱹ, �հ����� ����ִ� ������Ʈ�� 3�հ��� �̻��� ��� �ִ� ������ ������Ʈ�� �����س� 
        // 
        List<ChildObject_Custom> matchingScripts = childScripts.FindAll(script => script.grabbedObj == other && other != null);
        if (matchingScripts.Count >= 3)
        {
            isGrabbing = true;
            grabbedObject = other;
            grabbedObject.GetComponent<ObjectGrabable>().isGrabbed = true;
            grabbedObject.GetComponent<ObjectGrabable>().grabbingHand = handTransform;
            //   grabbedObject.GetComponent<ObjectGrabable>().initialGrabHandPosition = handTransform.position;
            grabbedObject.GetComponent<ObjectGrabable>().offset = handTransform.position - grabbedObject.transform.position;


            //  grabbedObject.GetComponent<ObjectGrabable>().lastHandPosition = handTransform.position;
            //  grabbedObject.GetComponent<ObjectGrabable>().lastHandRotation = handTransform.eulerAngles;


        }

    }
    private void FixedUpdate()
    {
        EvaluateRelease();

        if (this.isGrabbing)
        {
            EvaluateGrab();

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            isGrabbing = false;
            beingTouched = 0;
            if (grabbedObject != null)
            {
                grabbedObject.GetComponent<ObjectGrabable>().isGrabbed = false;
            }
            foreach (ChildObject_Custom childScript in childScripts)
            {
                // Assuming each child object has a GameObject property, you can deactivate it.
                childScript.gameObject.SetActive(false);
            }


        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (ChildObject_Custom childScript in childScripts)
            {
                // Assuming each child object has a GameObject property, you can activate it.
                childScript.gameObject.SetActive(true);
            }
        }
    }


    public void EvaluateGrab()
    {

    }
    public void EvaluateRelease()
    {
        if (grabbedObject != null)
        {
            if (beingTouched < 3)
            {
                //   Debug.Log(grabbedObject.GetComponent<Rigidbody>().velocity);
                grabbedObject.GetComponent<ObjectGrabable>().isGrabbed = false;
                grabbedObject = null;
                isGrabbing = false;
            }
        }
    }

}
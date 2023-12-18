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
        // 부모 오브젝트 아래에 있는 모든 자식 오브젝트의 ChildObject 스크립트를 가져옴
        childScripts.AddRange(GetComponentsInChildren<ChildObject_Custom>());

        // 각 자식 오브젝트에 대해 이벤트 핸들러 등록
        foreach (ChildObject_Custom childScript in childScripts)
        {
            // 각 자식 오브젝트의 이벤트 핸들러 등록
            childScript.OnColorChanged += HandleColorChanged;

        }

        InvokeRepeating("LogBeingTouchedRatio", 0f, 1.2f);

    }
    private void LogBeingTouchedRatio()
    {
        // 로그 출력
        //  Debug.Log(this.transform.name +"터치 중인 비율 = " + (beingTouched / childScripts.Count) * 100f + " % ");
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

        // 모든 손가락을 검사해 동일한 오브젝트를 쥔 갯수를 검사함
        // 감지된 갯수도 계산해서 2중 조건으로 null이 아닌 오브젝트를 검사하게 됨
        // 결국, 손가락이 닿아있는 오브젝트중 3손가락 이상이 닿아 있는 동일한 오브젝트만 검출해냄 
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
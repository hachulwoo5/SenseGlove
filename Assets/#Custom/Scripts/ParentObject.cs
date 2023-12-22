using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObject : MonoBehaviour
{
    public float beingTouched = 0;
    public float touchPercentage;
    public bool isReadyGrab;
    ChildObject[] childScripts;

    private void Start()
    {
        // 부모 오브젝트 아래에 있는 모든 자식 오브젝트의 ChildObject 스크립트를 가져옴
        childScripts = GetComponentsInChildren<ChildObject>();

        // 각 자식 오브젝트에 대해 이벤트 핸들러 등록
        foreach (ChildObject childScript in childScripts)
        {
            // 각 자식 오브젝트가 활성화되어 있는 경우에만 이벤트 핸들러 등록
            if (childScript.gameObject.activeSelf)
            {
                childScript.OnColorChanged += HandleColorChanged;
            }
        }

        InvokeRepeating("LogBeingTouchedRatio", 0f, 1.2f);

    }
    private void LogBeingTouchedRatio()
    {
        // 로그 출력
       // Debug.Log(this.transform.name +"터치 중인 비율 = " + (beingTouched / childScripts.Length) * 100f + " % ");
    }

    private void HandleColorChanged(Color newColor)
    {

        // 색깔이 바뀐 자식 오브젝트의 갯수 증가
        if (newColor == Color.green)
        {
            beingTouched++;
        }
        if (newColor == Color.red)
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


}
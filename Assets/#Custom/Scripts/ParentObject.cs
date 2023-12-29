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
        // 부모 오브젝트 아래에 있는 모든 자식 오브젝트의 ChildObject 스크립트를 가져옴
        childScripts = GetComponentsInChildren<ChildObject>();

        // 각 자식 오브젝트에 대해 이벤트 핸들러 등록
        foreach (ChildObject childScript in childScripts)
        {
            // 각 자식 오브젝트가 활성화되어 있는 경우에만 이벤트 핸들러 등록
            // 밑에서 색 변해서 이벤트 넘겨오면 HandleColorChanged가 발동해버림
            // 여기서 조작해줘
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
        // 로그 출력
       // Debug.Log(this.transform.name +"터치 중인 비율 = " + (beingTouched / childScripts.Length) * 100f + " % ");
    }

    private void HandleColorChanged(Color newColor)
    {
        Color targetColor1 = Color.red;
        Color targetColor2 = new Color(0x60 / 255f, 0 / 255f, 0xFF / 255f, 1f);

        // 색깔이 바뀐 자식 오브젝트의 갯수 증가
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

    // 차일드에서 사이드가 켜졌는지 여부를 가져와 이 bool(isSideGrab)에 적용시킨다
    private void SidePointChanged(bool isChildSide)
    {
        isSideGrab = isChildSide;

    }


}
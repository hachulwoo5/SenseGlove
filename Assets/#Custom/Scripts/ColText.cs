using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SG
{
    public class ColText : MonoBehaviour
    {
        public TextMeshProUGUI colText; // TextMeshProUGUI 사용
        public GameObject colTextObject; // 원본 GameObject 변수명 변경
        public int colTextnum;

        private void Update()
        {
          //colTextnum = colTextObject.GetComponent<SG_PhysicsGrab_Custom>().pointIndex;
          //colText.text = "Col: " + colTextnum.ToString(); // TMP의 text 속성을 사용하여 텍스트 설정
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SG
{
    public class ColText : MonoBehaviour
    {
        public TextMeshProUGUI colText; // TextMeshProUGUI ���
        public GameObject colTextObject; // ���� GameObject ������ ����
        public int colTextnum;

        private void Update()
        {
          //colTextnum = colTextObject.GetComponent<SG_PhysicsGrab_Custom>().pointIndex;
          //colText.text = "Col: " + colTextnum.ToString(); // TMP�� text �Ӽ��� ����Ͽ� �ؽ�Ʈ ����
        }
    }


}

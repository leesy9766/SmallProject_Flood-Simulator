using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DragManager : MonoBehaviour
{
    //���콺 ��Ŭ������ �巡�׾� ��� �� ���� ���õǴ� �巡�� �Ŵ���

    [SerializeField] Button_Panel button_panel;
    [SerializeField] private Camera UI_Camera;
    [SerializeField] private GameObject DragParent;
    [SerializeField] private Image squareImage;
    [SerializeField] private Image squareImagePrefab;
    [SerializeField] private GameObject Drag_obj;
    [SerializeField] private GameObject Drag_Prefab;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 deltaPos;
    [SerializeField] private Vector3 nowPos;

    float deltaX, deltaZ;

    [SerializeField] private float lengthX;
    [SerializeField] private float lengthY;
    [SerializeField] private Vector2 squarePos;

    public bool bcanDrag = false;

    private void Update()
    {
        if(bcanDrag)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //Ŭ���� ���� ������Ʈ Instancitate(), position�� startpos�� nowpos�� �߰���ǥ
                startPos = button_panel.GetMouseWorldPosition();
                Debug.Log(startPos);

                Drag_obj = Instantiate(Drag_Prefab, startPos, Quaternion.identity);
            }


            if (Input.GetMouseButton(1)) // �巡�� ��
            {              
                nowPos = button_panel.GetMouseWorldPosition();
                deltaX = Mathf.Abs(nowPos.x - startPos.x);
                deltaZ = Mathf.Abs(nowPos.z - startPos.z);
                deltaPos = startPos + (nowPos - startPos) / 2;
                Drag_obj.transform.position = deltaPos;
                Drag_obj.transform.localScale = new Vector3(deltaX, 10f, deltaZ);
            }


           // Debug.Log(startPos + " / " + nowPos);
            //if (Input.GetMouseButtonUp(0)) // �巡�װ� ������ ���� �簢�� ����
            //{
            //    Destroy(square_Prefab);
            //}

        }

    }

}

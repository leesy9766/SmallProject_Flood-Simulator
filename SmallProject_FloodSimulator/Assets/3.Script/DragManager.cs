using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DragManager : MonoBehaviour
{
    //���콺 ��Ŭ������ �巡�׾� ��� �� ���� ���õǴ� �巡�� �Ŵ���

    [Header("ETC")]
    [SerializeField] Button_Panel button_panel;
    [SerializeField] private Camera UI_Camera;


    //�θ������Ʈ--------------------------------------
    [Header("Parent Object")]
    [SerializeField] private GameObject DragImage_Parent;
    [SerializeField] private GameObject DragCollider_Parent;
    [SerializeField] private GameObject WaterPlane_Parent;


    //UI ȭ��� �巡�� �̹��� ������ ---------------------------
    [Header("Drag Image")]
    [SerializeField] private Image DragImage_obj;
    [SerializeField] private Image DragImage_Prefab;


    //World �� �巡�� �ݶ��̴� �ڽ�(��Ȧ �����) ������------------
    [Header("Drag Collider")]
    [SerializeField] private GameObject DragCollider_obj;
    [SerializeField] private GameObject DragCollider_Prefab;


    //�� ������-------------------------------------------------
    [Header("Water Plane")]
    [SerializeField] private GameObject WaterPlane_obj;
    [SerializeField] private GameObject WaterPlane_Prefab;
    
    private float waterPlanePosY = 1.4f;    //WaterPlane y�� ������


    //world�� �巡�� �� ��ǥ ����---------------------------
    [Header("Point Variable : World Object")]
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 deltaPos;
    [SerializeField] private Vector3 nowPos;
    float deltaX, deltaZ;


    [Header("Point Variable : UI")]
    private Vector2 startPos_UI;
    private Vector2 deltaPos_UI;
    private Vector2 nowPos_UI;
    float deltaX_UI, deltaY_UI;


    //UI ������Ʈ ũ�� ����----------------------------


    public bool bcanDrag = false;

    private void Update()
    {
        if(bcanDrag)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //Ŭ���� ���� ������Ʈ Instancitate(), position�� startpos�� nowpos�� �߰���ǥ
                startPos = button_panel.GetMouseWorldPosition();
                startPos_UI = Input.mousePosition;
                
                Debug.Log(startPos);

                DragCollider_obj = Instantiate(DragCollider_Prefab, startPos, Quaternion.identity);
                DragCollider_obj.transform.SetParent(DragCollider_Parent.transform);
                SystemManager.instance.DragObj_List.Add(DragCollider_obj);

                WaterPlane_obj = Instantiate(WaterPlane_Prefab, new Vector3(startPos.x, waterPlanePosY, startPos.z), Quaternion.identity);
                WaterPlane_obj.transform.SetParent(WaterPlane_Parent.transform);
                SystemManager.instance.WaterPlaneObj_List.Add(WaterPlane_obj);

                DragImage_obj = Instantiate(DragImage_Prefab, Vector3.zero, Quaternion.identity);
                DragImage_obj.transform.SetParent(DragImage_Parent.transform);
            }


            if (Input.GetMouseButton(1)) // �巡�� ��
            {              
                //���� ������Ʈ
                nowPos = button_panel.GetMouseWorldPosition();
                deltaX = Mathf.Abs(nowPos.x - startPos.x);
                deltaZ = Mathf.Abs(nowPos.z - startPos.z);
                deltaPos = startPos + (nowPos - startPos) / 2;

                //UI ������Ʈ
                nowPos_UI = Input.mousePosition;
                deltaX_UI = Math.Abs(nowPos_UI.x - startPos_UI.x);
                deltaY_UI = Math.Abs(nowPos_UI.y - startPos_UI.y);
                deltaPos_UI = startPos_UI + (nowPos_UI - startPos_UI) / 2;


                DragCollider_obj.transform.position = new Vector3(deltaPos.x, 0f, deltaPos.z);      //deltaPos;
                DragCollider_obj.transform.localScale = new Vector3(deltaX, 20f, deltaZ);

                WaterPlane_obj.transform.position = new Vector3(deltaPos.x, waterPlanePosY, deltaPos.z);
                WaterPlane_obj.transform.localScale = new Vector3(DragCollider_obj.transform.localScale.x, 0.1f, DragCollider_obj.transform.localScale.z);

                //��ģ���� UI�� Image ������Ʈ��
                DragImage_obj.rectTransform.position = deltaPos_UI;
                DragImage_obj.rectTransform.sizeDelta = new Vector2(deltaX_UI, deltaY_UI);         
                    
            }



            if (Input.GetMouseButtonUp(1)) // �巡�װ� ������ ���� �簢�� ����
            {
                Destroy(DragImage_obj);
            }

        }

    }

}

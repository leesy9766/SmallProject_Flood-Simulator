using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DragManager : MonoBehaviour
{
    //마우스 우클릭으로 드래그앤 드롭 시 지역 선택되는 드래그 매니저

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
                //클릭한 순간 오브젝트 Instancitate(), position은 startpos와 nowpos의 중간좌표
                startPos = button_panel.GetMouseWorldPosition();
                Debug.Log(startPos);

                Drag_obj = Instantiate(Drag_Prefab, startPos, Quaternion.identity);
            }


            if (Input.GetMouseButton(1)) // 드래그 중
            {              
                nowPos = button_panel.GetMouseWorldPosition();
                deltaX = Mathf.Abs(nowPos.x - startPos.x);
                deltaZ = Mathf.Abs(nowPos.z - startPos.z);
                deltaPos = startPos + (nowPos - startPos) / 2;
                Drag_obj.transform.position = deltaPos;
                Drag_obj.transform.localScale = new Vector3(deltaX, 10f, deltaZ);
            }


           // Debug.Log(startPos + " / " + nowPos);
            //if (Input.GetMouseButtonUp(0)) // 드래그가 끝나면 영역 사각형 삭제
            //{
            //    Destroy(square_Prefab);
            //}

        }

    }

}

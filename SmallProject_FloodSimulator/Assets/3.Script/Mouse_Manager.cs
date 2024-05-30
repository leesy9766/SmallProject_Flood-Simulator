using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct MousePos
{
    public float CurrentPosX;
    public float CurrentPosY;
    public Vector2 CurrentPos;
}

public class Mouse_Manager : MonoBehaviour
{
    [SerializeField] private Camera UI_Camera;
    public MousePos MouseInfo;

    //드래그 사각영역 관련 변수
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 CurrentPos;

    private void Update()
    {
        //MouseInfo.CurrentPosX = UI_Camera.ScreenToWorldPoint(Input.mousePosition).x;
        //MouseInfo.CurrentPosY = UI_Camera.ScreenToWorldPoint(Input.mousePosition).y;
        //MouseInfo.CurrentPos = new Vector2(MouseInfo.CurrentPosX, MouseInfo.CurrentPosY);



        if(Input.GetMouseButtonDown(1))
        {

        }

    }

}

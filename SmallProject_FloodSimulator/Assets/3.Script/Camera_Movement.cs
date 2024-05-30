using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [SerializeField] private Camera UI_Camera;

    //카메라 기뵌 위치 및 최대 최소 촬영 범위---------------------------------
    [SerializeField] private Vector3 BasicPos_Ortho = new Vector3(80f, 485f, 0f);
    [SerializeField] private Quaternion BasicRot_Ortho = Quaternion.Euler(90f, 0f, 181f);


    [SerializeField] private Vector3 BasicPos_Pers = new Vector3(-35f, 798f, -11.6f);
    [SerializeField] private Quaternion BasicRot_Pers = Quaternion.Euler(90.7f, -179.5f, 180.8f);


    private float MaxFieldofView = 450f;
    private float MinFieldofView = 10f;


    //카메라 회전관련------- ---------------------------------------------------
    private Vector3 previous_MousePos;
    private Vector3 current_MousePos;

    
    private bool rightclicked = false;  //좌클릭 되었는가 판단


    //[SerializeField] private float rotCamXAxisSpeed = 5f; // 카메라 x축 회전속도
    [SerializeField] private float rotCamYAxisSpeed = 3f; // 카메라 y축 회전속도

    private float eulerAngleX; // 마우스 좌 / 우 이동으로 카메라 y축 회전
    private float eulerAngleY; // 마우스 위 / 아래 이동으로 카메라 x축 회전



    //마우스 관련 --------------------------------------------------------------
    [SerializeField] public Vector2 CurrentMousePos;
    [SerializeField] private float CurrentMousePosX;
    [SerializeField] private float CurrentMousePosY;   //Y라곤 적었지만 3D상 Z값으로 연산하기 ㅇ.<

    //private float MouseWheelInput = 0f;
    private Vector2 MouseWheelInput;



    private void Awake()
    {
        UI_Camera = gameObject.GetComponent<Camera>();
    }


    private void Start()
    {
        Init();   
    }


    private void Update()
    {
        //Input.MousePos - 스크린포인트..
        CurrentMousePosX = UI_Camera.ScreenToWorldPoint(Input.mousePosition).x;
        CurrentMousePosY = UI_Camera.ScreenToWorldPoint(Input.mousePosition).y;
        CurrentMousePos = new Vector2(CurrentMousePosX, CurrentMousePosY);

       

        #region Zoom In/Out by mouse
        //카메라의 Field of View값/Size값 변경

        if (UI_Camera.orthographic)
        {
            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && UI_Camera.orthographicSize >= MinFieldofView)
            {
                //줌인
                Debug.Log("줌..!인..!!!!!");
                UI_Camera.orthographicSize -= 5f;

            }
            else if (MouseWheelInput.y < 0 && UI_Camera.orthographicSize <= MaxFieldofView)
            {
                //줌아웃
                Debug.Log("줌..!아웃..!!!!!");
                UI_Camera.orthographicSize += 5f;
            }

        }
        else
        {

            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && UI_Camera.fieldOfView >= 2f)
            {
                UI_Camera.transform.position = UI_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, UI_Camera.transform.position.y, Input.mousePosition.z));
                Debug.Log("줌..!인..!!!!!");
                UI_Camera.fieldOfView -= 2.5f;
            }
            else if (MouseWheelInput.y < 0 && UI_Camera.fieldOfView <= 60f)
            {
                //UI_Camera.transform.position = new Vector3(Input.mousePosition.x, UI_Camera.transform.position.y, Input.mousePosition.z);
                Debug.Log("줌..!아웃..!!!!!");
                UI_Camera.fieldOfView += 2.5f;
            }
        }


        #endregion



        #region Movement by keyboard
        if (Input.GetKey(KeyCode.W))
        {
            UI_Camera.transform.position += Vector3.forward;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            UI_Camera.transform.position += Vector3.back;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            UI_Camera.transform.position += Vector3.left;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            UI_Camera.transform.position += Vector3.right;
        }

        #endregion



        //#region rotation by mouse

        //if(Input.GetMouseButton(1))
        //{
        //    if(!rightclicked)
        //    {
        //        //우클릭 실행됨
        //        previous_MousePos = UI_Camera.ScreenToWorldPoint(Input.mousePosition);  //첫 우클릭 시 한번만 update
        //        rightclicked = true;
        //    }

            
        //    UpdateRotate();
        //}

        //#endregion
    }


    private void Init()
    {

        if (UI_Camera.orthographic)
        {
            UI_Camera.orthographicSize = 450f;
        }
        else
        {
            UI_Camera.fieldOfView = 60f;
            //UI_Camera.transform.position = new Vector3(-35f, 798f, -11.6f);
            //UI_Camera.transform.rotation = Quaternion.Euler(90.7f, -179.5f, 180.8f);
            UI_Camera.farClipPlane = 2000f;
        }
     
    }

    public void CalculateRotation(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;
      
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    // 카메라 x축 회전의 경우 회전 범위를 설정
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }


    void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        CalculateRotation(mouseX, mouseY);
    }


    //처음으로 버튼누르면 카메라 위치 기본 상공으로 이동
    public void Goto_Basic()
    {
        if (UI_Camera.orthographic)
        {
            UI_Camera.transform.position = BasicPos_Ortho;
            UI_Camera.transform.rotation = BasicRot_Ortho;
        }
        else
        {
            UI_Camera.transform.position = BasicPos_Pers;
            UI_Camera.transform.rotation = BasicRot_Pers;
        }
       
    }
}

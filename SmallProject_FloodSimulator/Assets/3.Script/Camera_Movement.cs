using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    enum Mode
    {
        SimulationView = 0,
        UIView
    }
    Mode mode;

    //[SerializeField] private Camera UI_Camera;

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
        // UI_Camera = gameObject.GetComponent<Camera>();

    }


    private void Start()
    {
        Init();
    }


    private void Update()
    {

        #region Zoom In/Out by mouse
        //카메라의 Field of View값/Size값 변경

        if (SystemManager.instance.UseCamera.orthographic)
        {
            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && SystemManager.instance.UseCamera.orthographicSize >= MinFieldofView)
            {
                //줌인
                Debug.Log("줌..!인..!!!!!");
                SystemManager.instance.UseCamera.orthographicSize -= 5f;

            }
            else if (MouseWheelInput.y < 0 && SystemManager.instance.UseCamera.orthographicSize <= MaxFieldofView)
            {
                //줌아웃
                Debug.Log("줌..!아웃..!!!!!");
                SystemManager.instance.UseCamera.orthographicSize += 5f;
            }

        }
        else
        {
            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && SystemManager.instance.UseCamera.fieldOfView >= 2f)
            {
                SystemManager.instance.UseCamera.transform.position = SystemManager.instance.UseCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, SystemManager.instance.UseCamera.transform.position.y, Input.mousePosition.z));
                Debug.Log("줌..!인..!!!!!");
                SystemManager.instance.UseCamera.fieldOfView -= 2.5f;
            }
            else if (MouseWheelInput.y < 0 && SystemManager.instance.UseCamera.fieldOfView <= 60f)
            {
                //UI_Camera.transform.position = new Vector3(Input.mousePosition.x, UI_Camera.transform.position.y, Input.mousePosition.z);
                Debug.Log("줌..!아웃..!!!!!");
                SystemManager.instance.UseCamera.fieldOfView += 2.5f;
            }
        }


        #endregion




        //Input.MousePos - 스크린포인트..
        CurrentMousePosX = SystemManager.instance.UseCamera.ScreenToWorldPoint(Input.mousePosition).x;
        CurrentMousePosY = SystemManager.instance.UseCamera.ScreenToWorldPoint(Input.mousePosition).y;
        CurrentMousePos = new Vector2(CurrentMousePosX, CurrentMousePosY);



        #region Movement by keyboard
        if (Input.GetKey(KeyCode.W))
        {
            SystemManager.instance.UseCamera.transform.position += Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            SystemManager.instance.UseCamera.transform.position += Vector3.back;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            SystemManager.instance.UseCamera.transform.position += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SystemManager.instance.UseCamera.transform.position += Vector3.right;
        }

        #endregion



        #region rotation by mouse
        if(SystemManager.instance.view == SystemManager.ViewMode.SimulationView)
        {
            if (Input.GetMouseButton(1))
            {
                UpdateRotate();
            }
        }
  
        #endregion




    }


    private void Init()
    {

        if (SystemManager.instance.UseCamera.orthographic)
        {
            SystemManager.instance.UseCamera.orthographicSize = 450f;
        }
        else
        {
            SystemManager.instance.UseCamera.fieldOfView = 60f;
            //UI_Camera.transform.position = new Vector3(-35f, 798f, -11.6f);
            //UI_Camera.transform.rotation = Quaternion.Euler(90.7f, -179.5f, 180.8f);
            SystemManager.instance.UseCamera.farClipPlane = 2000f;
        }

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

    public void CalculateRotation(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;

        SystemManager.instance.UseCamera.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }
    
    public void ResetCamera_Pos()
    {
        SystemManager.instance.UseCamera = SystemManager.instance.UI_Camera;

        if(SystemManager.instance.UI_Camera.orthographic)
        {
            SystemManager.instance.UI_Camera.transform.position = BasicPos_Ortho;
            SystemManager.instance.UI_Camera.transform.rotation = BasicRot_Ortho;
        }
        else
        {
            SystemManager.instance.UI_Camera.transform.position = BasicPos_Pers;
            SystemManager.instance.UI_Camera.transform.rotation = BasicRot_Pers;
        }

        Camera.main.transform.position = new Vector3(72f, 47f, -390f);
        Camera.main.transform.rotation = Quaternion.Euler(154.8f, -174.8f, 180f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ī�޶� �̵���Ű�鼭 �ٹٲٱ� �������ϱ�..�ΰ��� ���ô�..
public class Camera_Movement : MonoBehaviour
{
    #region Variable
    private enum Mode
    {
        SimulationView = 0,
        UIView
    }

    private Mode mode;

    //[SerializeField] private Camera UI_Camera;

    //ī�޶� ��� ��ġ �� �ִ� �ּ� �Կ� ����---------------------------------
    [SerializeField] private Vector3 BasicPos_Ortho = new Vector3(80f, 485f, 0f);
    [SerializeField] private Quaternion BasicRot_Ortho = Quaternion.Euler(90f, 0f, 181f);


    [SerializeField] private Vector3 BasicPos_Pers = new Vector3(-35f, 798f, -11.6f);
    [SerializeField] private Quaternion BasicRot_Pers = Quaternion.Euler(90.7f, -179.5f, 180.8f);


    private float MaxFieldofView = 1500f;
    private float MinFieldofView = 10f;


    //ī�޶� ȸ������----------------------------------------------------------
    private Vector3 previous_MousePos;
    private Vector3 current_MousePos;

    private float limitMinX = -50f;
    private float limitMaxX = 50f;
  
    [SerializeField] private float rotCamAxisSpeed = 3f; // ī�޶� y�� ȸ���ӵ�

    private float eulerAngleX; // ���콺 �� / �� �̵����� ī�޶� y�� ȸ��
    private float eulerAngleY; // ���콺 �� / �Ʒ� �̵����� ī�޶� x�� ȸ��

    //ī�޶� �̵� ����----------------------------------------------------------
    private float horizontal, vertical;
    private float moveSpeed = 100f;

    //���콺 ���� --------------------------------------------------------------
    [SerializeField] public Vector2 CurrentMousePos;
    [SerializeField] private float CurrentMousePosX;
    [SerializeField] private float CurrentMousePosY;  


    //private float MouseWheelInput = 0f;
    private Vector2 MouseWheelInput;
    #endregion


    #region Unity Event
    private void Start()
    {
        Init();
    }


    private void Update()
    {

        #region Zoom In/Out by mouse
        //ī�޶��� Field of View��/Size�� ����

        if (SystemManager.instance.UseCamera.orthographic)
        {
            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && SystemManager.instance.UseCamera.orthographicSize >= MinFieldofView)
            {
                //����      
                SystemManager.instance.UseCamera.orthographicSize -= 5f;
               

            }
            else if (MouseWheelInput.y < 0 && SystemManager.instance.UseCamera.orthographicSize <= MaxFieldofView)
            {
                //�ܾƿ�     
                SystemManager.instance.UseCamera.orthographicSize += 5f;
              
            }

          

        }
        else
        {
            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && SystemManager.instance.UseCamera.fieldOfView >= 2f)
            {
                //SystemManager.instance.UseCamera.transform.position = SystemManager.instance.UseCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, SystemManager.instance.UseCamera.transform.position.y, Input.mousePosition.z));
                SystemManager.instance.UseCamera.fieldOfView -= 2.5f;
            }
            else if (MouseWheelInput.y < 0 && SystemManager.instance.UseCamera.fieldOfView <= 60f)
            {
                //UI_Camera.transform.position = new Vector3(Input.mousePosition.x, UI_Camera.transform.position.y, Input.mousePosition.z);
                SystemManager.instance.UseCamera.fieldOfView += 2.5f;
            }
        }


        #endregion

        #region MousePoint
        //Input.MousePos - ��ũ������Ʈ..
        CurrentMousePosX = SystemManager.instance.UseCamera.ScreenToWorldPoint(Input.mousePosition).x;
        CurrentMousePosY = SystemManager.instance.UseCamera.ScreenToWorldPoint(Input.mousePosition).y;
        CurrentMousePos = new Vector2(CurrentMousePosX, CurrentMousePosY);
        #endregion

        #region Movement by keyboard
        horizontal = Input.GetAxis("Horizontal");
        vertical =  Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical);

        if (!(horizontal == 0 && vertical == 0))
        {
            Vector3 move = SystemManager.instance.UseCamera.transform.TransformDirection(transform.forward * vertical + transform.right * horizontal);
            SystemManager.instance.UseCamera.transform.position += move * Time.unscaledDeltaTime * moveSpeed;
        }
        #endregion

        #region rotation by mouse
        if (SystemManager.instance.view == SystemManager.ViewMode.SimulationView)
        {
            if (Input.GetMouseButton(1))
            {
                UpdateRotate();
            }
        }

        #endregion

    }

    #endregion



    #region Method

    private void Init()
    {

        if (SystemManager.instance.UseCamera.orthographic)
        {
            SystemManager.instance.UseCamera.orthographicSize = MaxFieldofView;
        }
        else
        {
            SystemManager.instance.UseCamera.fieldOfView = 60f;
            //UI_Camera.transform.position = new Vector3(-35f, 798f, -11.6f);
            //UI_Camera.transform.rotation = Quaternion.Euler(90.7f, -179.5f, 180.8f);
            SystemManager.instance.UseCamera.farClipPlane = 2000f;
        }

        moveSpeed = 20f;

    }


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
        eulerAngleY += mouseX * rotCamAxisSpeed;
        eulerAngleX -= mouseY * rotCamAxisSpeed;
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
        SystemManager.instance.UseCamera.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }
    

    public void ResetCamera_Pos()
    {
        SystemManager.instance.UseCamera = SystemManager.instance.UI_Camera;

        if(!SystemManager.instance.UI_Camera.orthographic)
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

    

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [SerializeField] private Camera UI_Camera;

    //ī�޶� ��� ��ġ �� �ִ� �ּ� �Կ� ����---------------------------------
    [SerializeField] private Vector3 BasicPos_Ortho = new Vector3(80f, 485f, 0f);
    [SerializeField] private Quaternion BasicRot_Ortho = Quaternion.Euler(90f, 0f, 181f);


    [SerializeField] private Vector3 BasicPos_Pers = new Vector3(-35f, 798f, -11.6f);
    [SerializeField] private Quaternion BasicRot_Pers = Quaternion.Euler(90.7f, -179.5f, 180.8f);


    private float MaxFieldofView = 450f;
    private float MinFieldofView = 10f;


    //ī�޶� ȸ������------- ---------------------------------------------------
    private Vector3 previous_MousePos;
    private Vector3 current_MousePos;

    
    private bool rightclicked = false;  //��Ŭ�� �Ǿ��°� �Ǵ�


    //[SerializeField] private float rotCamXAxisSpeed = 5f; // ī�޶� x�� ȸ���ӵ�
    [SerializeField] private float rotCamYAxisSpeed = 3f; // ī�޶� y�� ȸ���ӵ�

    private float eulerAngleX; // ���콺 �� / �� �̵����� ī�޶� y�� ȸ��
    private float eulerAngleY; // ���콺 �� / �Ʒ� �̵����� ī�޶� x�� ȸ��



    //���콺 ���� --------------------------------------------------------------
    [SerializeField] public Vector2 CurrentMousePos;
    [SerializeField] private float CurrentMousePosX;
    [SerializeField] private float CurrentMousePosY;   //Y��� �������� 3D�� Z������ �����ϱ� ��.<

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
        //Input.MousePos - ��ũ������Ʈ..
        CurrentMousePosX = UI_Camera.ScreenToWorldPoint(Input.mousePosition).x;
        CurrentMousePosY = UI_Camera.ScreenToWorldPoint(Input.mousePosition).y;
        CurrentMousePos = new Vector2(CurrentMousePosX, CurrentMousePosY);

       

        #region Zoom In/Out by mouse
        //ī�޶��� Field of View��/Size�� ����

        if (UI_Camera.orthographic)
        {
            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && UI_Camera.orthographicSize >= MinFieldofView)
            {
                //����
                Debug.Log("��..!��..!!!!!");
                UI_Camera.orthographicSize -= 5f;

            }
            else if (MouseWheelInput.y < 0 && UI_Camera.orthographicSize <= MaxFieldofView)
            {
                //�ܾƿ�
                Debug.Log("��..!�ƿ�..!!!!!");
                UI_Camera.orthographicSize += 5f;
            }

        }
        else
        {

            MouseWheelInput = Input.mouseScrollDelta;
            if (MouseWheelInput.y > 0 && UI_Camera.fieldOfView >= 2f)
            {
                UI_Camera.transform.position = UI_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, UI_Camera.transform.position.y, Input.mousePosition.z));
                Debug.Log("��..!��..!!!!!");
                UI_Camera.fieldOfView -= 2.5f;
            }
            else if (MouseWheelInput.y < 0 && UI_Camera.fieldOfView <= 60f)
            {
                //UI_Camera.transform.position = new Vector3(Input.mousePosition.x, UI_Camera.transform.position.y, Input.mousePosition.z);
                Debug.Log("��..!�ƿ�..!!!!!");
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
        //        //��Ŭ�� �����
        //        previous_MousePos = UI_Camera.ScreenToWorldPoint(Input.mousePosition);  //ù ��Ŭ�� �� �ѹ��� update
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

    // ī�޶� x�� ȸ���� ��� ȸ�� ������ ����
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


    //ó������ ��ư������ ī�޶� ��ġ �⺻ ������� �̵�
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

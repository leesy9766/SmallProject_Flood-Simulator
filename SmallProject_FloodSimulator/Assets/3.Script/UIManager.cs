using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    private Camera_Movement camera_movement;

    [Header("������ �Է�â")]
    [SerializeField] private TMP_InputField RainAmount_InputField;
    [SerializeField] private Button Confirm_Btn;

    [Header("��ư")]
    [SerializeField] private Button Manhole_Btn;
    [SerializeField] private Button ResetCamera_Btn;
    [SerializeField] private Button Simulation_Btn;
    [SerializeField] private Button SimulationView_Btn;
    [SerializeField] private Button ManholeShow_Btn;
    [SerializeField] private Button DragArea_Btn;
    [SerializeField] private Button PlaySpeed_Btn;
    [SerializeField] private Button Pause_Btn;
  
    [Header("��Ȧ ��ġ Ŀ��")]
    [SerializeField] GameObject ManholeImage_Parent;
    [SerializeField] Image ManholeImage_Prefab;
    [SerializeField] Image ManholeImage;

    [Header("��ư ����")]
    //��� �� ���� ����
    [SerializeField] private Sprite[] SpeedImageSprite_Arr;
    private Image PlaySpeed_Image;
    private TMP_Text PlaySpeed_Text;
    private int currentTimeScale;
    private bool bPause = false;

    //�巡�� ���� Ȱ��ȭ ����
    private bool bDragAreaActive = true;

    private enum PlaySpeed
    {
        x1 = 0,
        x2, 
        x3
    }

    private PlaySpeed playSpeed = 0;

    [SerializeField] private bool bManholeBtnClicked = false;    //��Ȧ ��ġ ��ư Ŭ�� ����

    private void Start()
    {
        Init();
    }


    private void Init()
    {
        //ī�޶� ������Ʈ------------------------------------------------------------------
        camera_movement = SystemManager.instance.UI_Camera.GetComponent<Camera_Movement>();

        //��ư �̺�Ʈ �޼ҵ� ����-----------------------------------------------------------
        Confirm_Btn.onClick.AddListener(ConfirmBtn_Clicked);
        Manhole_Btn.onClick.AddListener(ManholeBtn_Clicked);

        DragArea_Btn.onClick.AddListener(DragAreaBtn_Clicked);
        PlaySpeed_Btn.onClick.AddListener(PlaySpeedBtn_Clicked);
        Pause_Btn.onClick.AddListener(PauseBtn_Clicked);

        //��� �ӵ� ����------------------------------------------------------------------
        PlaySpeed_Image = PlaySpeed_Btn.transform.GetChild(0).GetComponent<Image>();
        PlaySpeed_Text = PlaySpeed_Btn.transform.GetChild(1).GetComponent<TMP_Text>();


        //���� �⺻ ����-------------------------------------------------------------------
        playSpeed = PlaySpeed.x1;
        PlaySpeed_Image.sprite = SpeedImageSprite_Arr[0];
        PlaySpeed_Text.text = "1���";
        currentTimeScale = 1;
        bPause = false;
        bDragAreaActive = true;
      
    }

    private void Update()
    {
        if (ManholeImage != null)
        {
            ManholeImage.rectTransform.position = Input.mousePosition;
            SystemManager.instance.bCanManholeCreate = true;
        }
        else
        {
            SystemManager.instance.bCanManholeCreate = false;
        }
    }


    #region ��ư�޼ҵ�
    public void ConfirmBtn_Clicked()
    {
        if(RainAmount_InputField.text == string.Empty)
        {
            SystemManager.instance.RainAmount = 0f;
        }
        else
        {
            SystemManager.instance.RainAmount = int.Parse(RainAmount_InputField.text);
        }
    }


    private void ManholeBtn_Clicked()
    {
        if (!bManholeBtnClicked)
        {
            //��ư�� �ȴ����� ��
            bManholeBtnClicked = true;
            Manhole_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "�Ϲݸ��";


            //Ŀ���� ����ٴϴ� Prefab
            ManholeImage = Instantiate(ManholeImage_Prefab, camera_movement.CurrentMousePos, Quaternion.identity);
            ManholeImage.transform.SetParent(ManholeImage_Parent.transform);
        }
        else
        {
            //��ư�� ������ ��
            bManholeBtnClicked = false;

            Manhole_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "��Ȧ ��ġ";
            Destroy(ManholeImage);

        }
    }


    //to. �������� ������ - ���İ� �ǵ����� ��Ƽ���� �÷� �ȵ��ƿ´�..���Ķ�..
    private void DragAreaBtn_Clicked()
    {
        //DragObject_List�� ��� ������ Material�� ���İ��� 0

        if(bDragAreaActive)
        {

            

            //���� ���ø��� off
            bDragAreaActive = false;
            for (int i = 0; i < SystemManager.instance.DragObj_List.Count; i++)
            {
                SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].color = new Color(255, 241, 81, 0);
                //SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].
            }
        }
        else
        {
            //���� �񰡽ø��� on
            bDragAreaActive = true;
            for (int i = 0; i < SystemManager.instance.DragObj_List.Count; i++)
            {
                SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].color = new Color(255, 241, 81, 80);
            }
        }
      
    }



    private void PlaySpeedBtn_Clicked()
    {
        //��� ������ timescale 5�� ����
        if(playSpeed == PlaySpeed.x1)
        {
            playSpeed = PlaySpeed.x2;
            currentTimeScale = 5;
           
            PlaySpeed_Image.sprite = SpeedImageSprite_Arr[1];
            PlaySpeed_Text.text = "2���";
        }
        else if(playSpeed == PlaySpeed.x2)
        {
            playSpeed = PlaySpeed.x3;
            currentTimeScale = 10;
            
            PlaySpeed_Image.sprite = SpeedImageSprite_Arr[2];
            PlaySpeed_Text.text = "3���";
        }
        else
        {
            playSpeed = PlaySpeed.x1;
            currentTimeScale = 1;
            PlaySpeed_Image.sprite = SpeedImageSprite_Arr[0];
            PlaySpeed_Text.text = "1���";
        }


        Time.timeScale = currentTimeScale;
    }


    private void PauseBtn_Clicked()
    {
        if(!bPause)
        {
            bPause = true;
            Pause_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "�ٽ� ����";
            Time.timeScale = 0f;
        }
        else
        {
            bPause = false;
            Pause_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "����";
            Time.timeScale = currentTimeScale;
        }
        
    }
    #endregion


}

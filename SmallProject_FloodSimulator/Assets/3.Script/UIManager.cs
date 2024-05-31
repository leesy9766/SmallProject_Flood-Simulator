using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    private Camera_Movement camera_movement;

    [Header("강수량 입력창")]
    [SerializeField] private TMP_InputField RainAmount_InputField;
    [SerializeField] private Button Confirm_Btn;

    [Header("버튼")]
    [SerializeField] private Button Manhole_Btn;
    [SerializeField] private Button ResetCamera_Btn;
    [SerializeField] private Button Simulation_Btn;
    [SerializeField] private Button SimulationView_Btn;
    [SerializeField] private Button ManholeShow_Btn;
    [SerializeField] private Button DragArea_Btn;
    [SerializeField] private Button PlaySpeed_Btn;
    [SerializeField] private Button Pause_Btn;
  
    [Header("맨홀 설치 커서")]
    [SerializeField] GameObject ManholeImage_Parent;
    [SerializeField] Image ManholeImage_Prefab;
    [SerializeField] Image ManholeImage;

    [Header("버튼 설정")]
    //배속 및 정지 관련
    [SerializeField] private Sprite[] SpeedImageSprite_Arr;
    private Image PlaySpeed_Image;
    private TMP_Text PlaySpeed_Text;
    private int currentTimeScale;
    private bool bPause = false;

    //드래그 영역 활성화 여부
    private bool bDragAreaActive = true;

    private enum PlaySpeed
    {
        x1 = 0,
        x2, 
        x3
    }

    private PlaySpeed playSpeed = 0;

    [SerializeField] private bool bManholeBtnClicked = false;    //맨홀 설치 버튼 클릭 여부

    private void Start()
    {
        Init();
    }


    private void Init()
    {
        //카메라 컴포넌트------------------------------------------------------------------
        camera_movement = SystemManager.instance.UI_Camera.GetComponent<Camera_Movement>();

        //버튼 이벤트 메소드 연결-----------------------------------------------------------
        Confirm_Btn.onClick.AddListener(ConfirmBtn_Clicked);
        Manhole_Btn.onClick.AddListener(ManholeBtn_Clicked);

        DragArea_Btn.onClick.AddListener(DragAreaBtn_Clicked);
        PlaySpeed_Btn.onClick.AddListener(PlaySpeedBtn_Clicked);
        Pause_Btn.onClick.AddListener(PauseBtn_Clicked);

        //배속 속도 관련------------------------------------------------------------------
        PlaySpeed_Image = PlaySpeed_Btn.transform.GetChild(0).GetComponent<Image>();
        PlaySpeed_Text = PlaySpeed_Btn.transform.GetChild(1).GetComponent<TMP_Text>();


        //변수 기본 설정-------------------------------------------------------------------
        playSpeed = PlaySpeed.x1;
        PlaySpeed_Image.sprite = SpeedImageSprite_Arr[0];
        PlaySpeed_Text.text = "1배속";
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


    #region 버튼메소드
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
            //버튼이 안눌렸을 때
            bManholeBtnClicked = true;
            Manhole_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "일반모드";


            //커서를 따라다니는 Prefab
            ManholeImage = Instantiate(ManholeImage_Prefab, camera_movement.CurrentMousePos, Quaternion.identity);
            ManholeImage.transform.SetParent(ManholeImage_Parent.transform);
        }
        else
        {
            //버튼이 눌렸을 때
            bManholeBtnClicked = false;

            Manhole_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "맨홀 설치";
            Destroy(ManholeImage);

        }
    }


    //to. 다음주의 나에게 - 알파값 되돌릴때 머티리얼 컬러 안돌아온다..고쳐라..
    private void DragAreaBtn_Clicked()
    {
        //DragObject_List의 모든 원소의 Material의 알파값을 0

        if(bDragAreaActive)
        {

            

            //영역 가시모드면 off
            bDragAreaActive = false;
            for (int i = 0; i < SystemManager.instance.DragObj_List.Count; i++)
            {
                SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].color = new Color(255, 241, 81, 0);
                //SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].
            }
        }
        else
        {
            //영역 비가시모드면 on
            bDragAreaActive = true;
            for (int i = 0; i < SystemManager.instance.DragObj_List.Count; i++)
            {
                SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].color = new Color(255, 241, 81, 80);
            }
        }
      
    }



    private void PlaySpeedBtn_Clicked()
    {
        //배속 증가당 timescale 5씩 증가
        if(playSpeed == PlaySpeed.x1)
        {
            playSpeed = PlaySpeed.x2;
            currentTimeScale = 5;
           
            PlaySpeed_Image.sprite = SpeedImageSprite_Arr[1];
            PlaySpeed_Text.text = "2배속";
        }
        else if(playSpeed == PlaySpeed.x2)
        {
            playSpeed = PlaySpeed.x3;
            currentTimeScale = 10;
            
            PlaySpeed_Image.sprite = SpeedImageSprite_Arr[2];
            PlaySpeed_Text.text = "3배속";
        }
        else
        {
            playSpeed = PlaySpeed.x1;
            currentTimeScale = 1;
            PlaySpeed_Image.sprite = SpeedImageSprite_Arr[0];
            PlaySpeed_Text.text = "1배속";
        }


        Time.timeScale = currentTimeScale;
    }


    private void PauseBtn_Clicked()
    {
        if(!bPause)
        {
            bPause = true;
            Pause_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "다시 실행";
            Time.timeScale = 0f;
        }
        else
        {
            bPause = false;
            Pause_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "정지";
            Time.timeScale = currentTimeScale;
        }
        
    }
    #endregion


}

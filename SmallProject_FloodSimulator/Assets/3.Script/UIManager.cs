using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Camera_Movement camera_movement;
    [SerializeField] private DragManager dragManager;

    [Header("강수량 입력창")]
    [SerializeField] private TMP_InputField RainAmount_InputField;
    [SerializeField] private Button Confirm_Btn;
    [SerializeField] private TMP_Text Number_Text;

    [Header("버튼")]
    [SerializeField] private Button Manhole_Btn;
    [SerializeField] private Button Reset_Btn;
    [SerializeField] private Button Simulation_Btn;
    [SerializeField] private Button ViewMode_Btn;
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

    //시뮬레이션 관련
    [Header("시뮬레이션 관련")]
    [SerializeField] private GameObject RawImage_Panel;
    private enum PlaySpeed
    {
        x1 = 0,
        x2, 
        x3
    }

    private PlaySpeed playSpeed = 0;

    [SerializeField] private bool bManholeBtnClicked = false;    //맨홀 설치 버튼 클릭 여부


    //맨홀 찾기 관련
    [Header("맨홀 찾기 관련")]
    [SerializeField] private List<Image> img_List = null;

    [SerializeField] private GameObject manholeImage_Parent;
    [SerializeField] private Image manholeImage_Prefab;
    [SerializeField] private Sprite manholeIcon_Sprite;

    private bool bisActive = false;


    private void Start()
    {
        Init();
    }


    private void Init()
    {
        //카메라 컴포넌트------------------------------------------------------------------
       

        //버튼 이벤트 메소드 연결-----------------------------------------------------------
        Confirm_Btn.onClick.AddListener(ConfirmBtn_Clicked);
        Reset_Btn.onClick.AddListener(ResetBtn_Clicked);
        Simulation_Btn.onClick.AddListener(SimulationBtn_Clicked);
        Manhole_Btn.onClick.AddListener(ManholeBtn_Clicked);
        ViewMode_Btn.onClick.AddListener(ViewModeBtn_Clicked);
        ManholeShow_Btn.onClick.AddListener(ManholeShowBtn_Clicked);
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

        //UI 기본설정 -------------------------------------------------------------------
        if(Number_Text.gameObject.activeSelf)
        {
            Number_Text.gameObject.SetActive(false);
        }

      
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

        Number_Text.text = SystemManager.instance.RainAmount.ToString() + " mm/s";

        RainAmount_InputField.gameObject.SetActive(false);
        Confirm_Btn.gameObject.SetActive(false);
        Number_Text.gameObject.SetActive(true);
    }

    public void ResetBtn_Clicked()
    {
        for(int i = 0; i<SystemManager.instance.DragObj_List.Count; i++)
        {
            Destroy(SystemManager.instance.DragObj_List[i].gameObject);           
        }
        for (int i = 0; i < SystemManager.instance.WaterPlaneObj_List.Count; i++)
        {
            Destroy(SystemManager.instance.WaterPlaneObj_List[i].gameObject);        
        }
        for (int i = 0; i < SystemManager.instance.ManholeObj_List.Count; i++)
        {
            Destroy(SystemManager.instance.ManholeObj_List[i].gameObject);        
        }


        SystemManager.instance.DragObj_List.Clear();
        SystemManager.instance.WaterPlaneObj_List.Clear();
        SystemManager.instance.ManholeObj_List.Clear();

        camera_movement.ResetCamera_Pos();

        //UI오브젝트 ON/OFF
        Number_Text.gameObject.SetActive(false);
        Confirm_Btn.gameObject.SetActive(true);
        RainAmount_InputField.gameObject.SetActive(true);
        RainAmount_InputField.text = string.Empty;

        //변수값 초기화
        SystemManager.instance.RainAmount = 0f;


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


    private void ViewModeBtn_Clicked()
    {
        if (SystemManager.instance.view == SystemManager.ViewMode.UIView)
        {
            //UI뷰면 시뮬뷰로 변경
            SystemManager.instance.view = SystemManager.ViewMode.SimulationView;
            SystemManager.instance.UseCamera = Camera.main;
            ViewMode_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "UI 뷰";    
            RawImage_Panel.SetActive(false);
        }
        else if(SystemManager.instance.view == SystemManager.ViewMode.SimulationView)
        {
            SystemManager.instance.view = SystemManager.ViewMode.UIView;
            SystemManager.instance.UseCamera = SystemManager.instance.UI_Camera;
            ViewMode_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "3D 뷰";
            RawImage_Panel.SetActive(true);
        }
    }


    private void SimulationBtn_Clicked()
    {
        dragManager.Instanciate_WaterPlane();
        SystemManager.instance.Flooding();
    }

    private void ManholeShowBtn_Clicked()
    {
        if(!bisActive)
        {
            bisActive = true;
            img_List.Clear();
            for (int i = 0; i < SystemManager.instance.ManholeObj_List.Count; i++)
            {
                Vector3 t = SystemManager.instance.UI_Camera.WorldToScreenPoint(SystemManager.instance.ManholeObj_List[i].transform.position);
                Image img = Instantiate(manholeImage_Prefab, t, Quaternion.identity);
                img.transform.SetParent(manholeImage_Parent.transform);
                //img.sprite = manholeIcon_Sprite;
                img_List.Add(img);
            }

            ManholeShow_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "맨홀위치 \n 끄기";
        }
        else
        {
            bisActive = false;
            for(int i =0; i<img_List.Count; i++)
            {
                Destroy(img_List[i].gameObject);
                
            }
            img_List.Clear();

            ManholeShow_Btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "맨홀위치 \n 확인";
        }
     
    }


    //to. 내일의 나에게 - 알파값 되돌릴때 머티리얼 컬러 안돌아온다..고쳐라..
    private void DragAreaBtn_Clicked()
    {
        //DragObject_List의 모든 원소의 Material의 알파값을 0 -> 일단 그냥 오브젝트 껏다..켯다..

        

        if(bDragAreaActive)
        {
            //영역 가시모드면 off
            bDragAreaActive = false;
            for (int i = 0; i < SystemManager.instance.DragObj_List.Count; i++)
            {
               

                SystemManager.instance.DragObj_List[i].gameObject.SetActive(false);

                //SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].color = new Color(255, 241, 81, 0);

            }
        }
        else
        {
            //영역 비가시모드면 on
            bDragAreaActive = true;
            for (int i = 0; i < SystemManager.instance.DragObj_List.Count; i++)
            {
               
                SystemManager.instance.DragObj_List[i].gameObject.SetActive(true);

                //SystemManager.instance.DragObj_List[i].GetComponent<MeshRenderer>().materials[0].color = new Color(255, 241, 81, 80);
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
            PlaySpeed_Text.text = "5배속";
        }
        else if(playSpeed == PlaySpeed.x2)
        {
            playSpeed = PlaySpeed.x3;
            currentTimeScale = 10;
            
            PlaySpeed_Image.sprite = SpeedImageSprite_Arr[2];
            PlaySpeed_Text.text = "10배속";
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//귀찮으니 일단 전역..싱글턴..
public class SystemManager : MonoBehaviour
{
    public static SystemManager instance = null;


    public enum ViewMode
    {
        SimulationView = 0,
        UIView
    }
    public ViewMode view;
    [Header("빌딩")]
    [SerializeField] private GameObject Building;

    [Header("카메라")]
    [SerializeField] public Camera UI_Camera;
    public Camera UseCamera;

    [Header("머터리얼")]
    public Material NormalBuilding_M;
    public Material FloodWatch_M;
    public Material FloodWarning_M;
    public Material Transparent_M;
    public Material Water_M;

    [Header("boolean 변수")]
    public bool bSimulating = false;        //시뮬레이션 중인가 아닌가 판단하는 변수
    public bool bManholeShow = false;       //맨홀위치 UI에 뜨도록 하는 변수
    public bool bCanManholeCreate = false;   //맨홀 생성이 가능한가 여부
    public bool bWaterAreaShow = true;     //물 영역 보여지는지 판단하는 변수
    public bool bColoredBuidlingShow = true;    //빌딩 색상 침수에따라 변경모드인지 아닌지 판단하는 변수
    

    [Header("수치 변수")]
    public float simulationTime = 10f;
    public float currentTime = 0f;

    [Header("드래그 오브젝트 리스트")]
    public List<WaterPlane> WaterPlaneObj_List = null;
    public List<GameObject> DragObj_List = null;

    [Header("맨홀 오브젝트 리스트")]
    public List<GameObject> ManholeObj_List = null;

    [Header("맨홀 체크 리스트")]
    public List<Manhole_Checker> ManholeCheckers_List = null;

    [Header("빌딩 리스트")]
    public List<Building> Building_List = null;

    //환경변수
    public float RainAmount = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Init();
    }



    private void Init()
    {
        Screen.SetResolution(1500, 1000, true);

        UseCamera = UI_Camera;
        view = ViewMode.UIView;

        WaterPlaneObj_List = new List<WaterPlane>();
        ManholeObj_List = new List<GameObject>(); 
        DragObj_List = new List<GameObject>();
       
        ManholeCheckers_List = new List<Manhole_Checker>();
        Building_List = new List<Building>();

        bSimulating = false;
        bManholeShow = false;
        bCanManholeCreate = false;
        bWaterAreaShow = true;
        bColoredBuidlingShow = true;

        InitBuildingList();
    }
        

    private void InitBuildingList()
    {
        
        for(int i =0; i<Building.transform.childCount; i++)
        {
            Building_List.Add(Building.transform.GetChild(i).GetComponent<Building>());
        }
    }

    public void InitBuildingColor()
    {
        for (int i = 0; i < Building_List.Count; i++)
        {
            if(Building_List[i].GetComponent<MeshRenderer>().material != NormalBuilding_M)
            {
                Building_List[i].GetComponent<MeshRenderer>().material = NormalBuilding_M;
            }
        }
    }

    public IEnumerator SimulationTimer_co()
    {
        currentTime = 0f;
        int cashing = 1;

        bSimulating = true;

        while(true)
        {
            if (currentTime >= simulationTime)
            {
                bSimulating = false;
                yield break;
            }

            currentTime += cashing;

            yield return new WaitForSeconds(cashing);
        }
    }


    public void Flooding()
    {
        for(int i = 0; i< DragObj_List.Count; i++)
        {
            Manhole_Checker checker = new Manhole_Checker();
            checker = DragObj_List[i].GetComponent<Manhole_Checker>();
        

            for (int j = 0; j< checker.Manhole_List.Count; j++)
            {
                WaterPlaneObj_List[i].StorageAmount += checker.Manhole_List[j].MaxWaterStarage;
                WaterPlaneObj_List[i].ManholeCount += 1;
            }
        }
    }


}

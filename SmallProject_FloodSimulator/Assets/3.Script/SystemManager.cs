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


    [SerializeField] public Camera UI_Camera;
    public Camera UseCamera;

    public bool bSimulating = false;        //시뮬레이션 여부 (WaterPlane)이 움직이게 하는 변수
    public bool bManholeShow = false;       //맨홀위치 UI에 뜨도록 하는 변수
    public bool bCanManholeCreate = false;   //맨홀 생성이 가능한가 여부
    public float simulationTime = 10f;
    public float currentTime = 0f;

    public List<WaterPlane> WaterPlaneObj_List = null;
    public List<GameObject> DragObj_List = null;
    public List<GameObject> ManholeObj_List = null;

    //드래그 영역 안에 있는 맨홀들 : Drag Collider의 맨홀 List를 관리하는 List -> 이중리스트
    public List<List<Manhole>> ManholeDragArea_List = null;
    public List<Manhole_Checker> ManholeCheckers_List = null;

    public List<float> TEST_LIST = null;

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
        ManholeDragArea_List = new List<List<Manhole>>();
        ManholeCheckers_List = new List<Manhole_Checker>();
        TEST_LIST = new List<float>();
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
            //ManholeCheckers_List.Add(checker);

            for (int j = 0; j< checker.Manhole_List.Count; j++)
            {
                WaterPlaneObj_List[i].StorageAmount += checker.Manhole_List[j].MaxWaterStarage;
                WaterPlaneObj_List[i].ManholeCount += 1;
            }


            TEST_LIST.Add(WaterPlaneObj_List[i].StorageAmount);
            
        }



    



    }
}

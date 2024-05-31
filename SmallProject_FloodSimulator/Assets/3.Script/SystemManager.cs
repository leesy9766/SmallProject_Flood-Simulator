using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager instance = null;

    [SerializeField] public Camera UI_Camera;

    public bool bSimulating = false;        //시뮬레이션 여부 (WaterPlane)이 움직이게 하는 변수
    public bool bManholeShow = false;       //맨홀위치 UI에 뜨도록 하는 변수
    public bool bCanManholeCreate = false;   //맨홀 생성이 가능한가 여부
    public float simulationTime = 10f;
    public float currentTime = 0f;

    public List<GameObject> WaterPlaneObj_List = null;
    public List<GameObject> DragObj_List = null;
    public List<GameObject> ManholeObj_List = null;

    //드래그 영역 안에 있는 맨홀들 : Drag Collider의 맨홀 List를 관리하는 List -> 이중리스트
    public List<List<Manhole>> ManholeDragArea_List = null;

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
    }


    private void Start()
    {
        Init();
    }



    private void Init()
    {
        WaterPlaneObj_List = new List<GameObject>();
        ManholeObj_List = new List<GameObject>(); 
        DragObj_List = new List<GameObject>();
        ManholeDragArea_List = new List<List<Manhole>>(); 
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

    //맨홀 드래그 리스트 가져오는 함수
    public void GetManholeInDragArea()
    {

    }
}

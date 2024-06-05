using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�������� �ϴ� ����..�̱���..
public class SystemManager : MonoBehaviour
{
    public static SystemManager instance = null;


    public enum ViewMode
    {
        SimulationView = 0,
        UIView
    }
    public ViewMode view;
    [Header("����")]
    [SerializeField] private GameObject Building;

    [Header("ī�޶�")]
    [SerializeField] public Camera UI_Camera;
    public Camera UseCamera;

    [Header("���͸���")]
    public Material NormalBuilding_M;
    public Material FloodWatch_M;
    public Material FloodWarning_M;
    public Material Transparent_M;
    public Material Water_M;

    [Header("boolean ����")]
    public bool bSimulating = false;        //�ùķ��̼� ���ΰ� �ƴѰ� �Ǵ��ϴ� ����
    public bool bManholeShow = false;       //��Ȧ��ġ UI�� �ߵ��� �ϴ� ����
    public bool bCanManholeCreate = false;   //��Ȧ ������ �����Ѱ� ����
    public bool bWaterAreaShow = true;     //�� ���� ���������� �Ǵ��ϴ� ����
    public bool bColoredBuidlingShow = true;    //���� ���� ħ�������� ���������� �ƴ��� �Ǵ��ϴ� ����
    

    [Header("��ġ ����")]
    public float simulationTime = 10f;
    public float currentTime = 0f;

    [Header("�巡�� ������Ʈ ����Ʈ")]
    public List<WaterPlane> WaterPlaneObj_List = null;
    public List<GameObject> DragObj_List = null;

    [Header("��Ȧ ������Ʈ ����Ʈ")]
    public List<GameObject> ManholeObj_List = null;

    [Header("��Ȧ üũ ����Ʈ")]
    public List<Manhole_Checker> ManholeCheckers_List = null;

    [Header("���� ����Ʈ")]
    public List<Building> Building_List = null;

    //ȯ�溯��
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

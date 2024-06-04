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


    [SerializeField] public Camera UI_Camera;
    public Camera UseCamera;

    public bool bSimulating = false;        //�ùķ��̼� ���� (WaterPlane)�� �����̰� �ϴ� ����
    public bool bManholeShow = false;       //��Ȧ��ġ UI�� �ߵ��� �ϴ� ����
    public bool bCanManholeCreate = false;   //��Ȧ ������ �����Ѱ� ����
    public float simulationTime = 10f;
    public float currentTime = 0f;

    public List<WaterPlane> WaterPlaneObj_List = null;
    public List<GameObject> DragObj_List = null;
    public List<GameObject> ManholeObj_List = null;

    //�巡�� ���� �ȿ� �ִ� ��Ȧ�� : Drag Collider�� ��Ȧ List�� �����ϴ� List -> ���߸���Ʈ
    public List<List<Manhole>> ManholeDragArea_List = null;
    public List<Manhole_Checker> ManholeCheckers_List = null;

    public List<float> TEST_LIST = null;

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

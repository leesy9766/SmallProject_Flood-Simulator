using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager instance = null;

    [SerializeField] public Camera UI_Camera;

    public bool bSimulating = false;        //�ùķ��̼� ���� (WaterPlane)�� �����̰� �ϴ� ����
    public bool bManholeShow = false;       //��Ȧ��ġ UI�� �ߵ��� �ϴ� ����
    public bool bCanManholeCreate = false;   //��Ȧ ������ �����Ѱ� ����
    public float simulationTime = 10f;
    public float currentTime = 0f;

    public List<GameObject> WaterPlaneObj_List = null;
    public List<GameObject> DragObj_List = null;
    public List<GameObject> ManholeObj_List = null;

    //�巡�� ���� �ȿ� �ִ� ��Ȧ�� : Drag Collider�� ��Ȧ List�� �����ϴ� List -> ���߸���Ʈ
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

    //��Ȧ �巡�� ����Ʈ �������� �Լ�
    public void GetManholeInDragArea()
    {

    }
}

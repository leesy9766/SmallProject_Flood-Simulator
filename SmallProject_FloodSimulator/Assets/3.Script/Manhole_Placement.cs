using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//��Ȧ ��ġ
public class Button_Panel : MonoBehaviour
{
    [SerializeField] Camera UI_Camera;
    

    [SerializeField] GameObject Manhole_Parent;

    
    [SerializeField] GameObject Manhole_Prefab;     //��Ȧ ������
   
    [SerializeField] List<Image> ManholeImage_List = null;
    [SerializeField] List<GameObject> Manhole_List = null;

    

    [SerializeField] LayerMask ModelLayer;
    public Vector3 ClickPoint { get; private set; }
    private Vector3 TestPoint;
    [SerializeField] private RaycastHit hit;

    private void Start()
    {
        Init();
    }


    private void Update()
    {
    
        if (SystemManager.instance.bCanManholeCreate && Input.GetMouseButtonDown(0))
        { 

            ClickPoint = GetMouseWorldPosition();          
            if (Physics.Raycast(ClickPoint, transform.up * -1, out hit, Mathf.Infinity, ModelLayer))
            {
                Debug.Log("hit point : " + hit.point + "/" + ClickPoint);

                Debug.Log(hit.transform.gameObject);

                if(hit.transform.gameObject.CompareTag("Ground"))
                {
                    GameObject obj = Instantiate(Manhole_Prefab, hit.point, Quaternion.identity);
                    obj.transform.SetParent(Manhole_Parent.transform);
                    SystemManager.instance.ManholeObj_List.Add(obj);
                }
          
            }
    
        }
    }


    private void Init()
    {

        ManholeImage_List = new List<Image>();
        Manhole_List = new List<GameObject>();
    }


    //Perspective ī�޶� - ���콺 ��ġ ����
    public Vector3 GetMouseWorldPosition()
    {
        // ���콺 ��ġ���� �����ϴ� ������ ����
        Ray ray = UI_Camera.ScreenPointToRay(Input.mousePosition);

        // ������ yPlane(0)�� ������ ���� ���
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }
        else
        {
           
            //// �⺻������ ������ ������ ���
            //Plane plane = new Plane(Vector3.up, new Vector3(0, UI_Camera.transform.position.y - 10f, 0)); ;
            //if (plane.Raycast(ray, out float distance))
            //{
            //    return ray.GetPoint(distance);
            //}
        }

        return Vector3.zero;
    }



}

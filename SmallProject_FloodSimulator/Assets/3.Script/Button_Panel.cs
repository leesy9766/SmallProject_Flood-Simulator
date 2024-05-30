using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_Panel : MonoBehaviour
{
    [SerializeField] Camera UI_Camera;
    [SerializeField] Camera_Movement camera_movement;
    [SerializeField] GameObject ManholeImage_Parent;
    [SerializeField] GameObject Manhole_Parent;

    [SerializeField] Button Manhole_Btn;
    [SerializeField] GameObject Manhole_Prefab;     //맨홀 프리팹
    [SerializeField] Image ManholeImage_Prefab;
    [SerializeField] Image ManholeImage;
    [SerializeField] List<Image> ManholeImage_List = null;
    [SerializeField] List<GameObject> Manhole_List = null;

    [SerializeField] private bool bManholeBtnClicked = false;    //맨홀 설치 버튼 클릭 여부
    private bool bCanManholeCreate = false;   //맨홀 생성이 가능한가 여부

    [SerializeField] LayerMask ModelLayer;
    public Vector3 ClickPoint { get; private set; }
    private Vector3 TestPoint;
    [SerializeField] private RaycastHit hit;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Manhole_Btn.onClick.AddListener(ManholeBtn_Clicked);
        ManholeImage_List = new List<Image>();
        Manhole_List = new List<GameObject>();
    }

    private void Update()
    {
        if(ManholeImage != null)
        {
            ManholeImage.rectTransform.position = Input.mousePosition;
            bCanManholeCreate = true;
        }
        else
        {
            bCanManholeCreate = false;
        }
     


        if (bCanManholeCreate && Input.GetMouseButtonDown(0))
        {
            //bCanManholeCreate = false;
            //Image img = Instantiate(ManholeImage_Prefab, Vector3.zero, Quaternion.identity);
            //img.rectTransform.position = Input.mousePosition;
            //img.transform.SetParent(ManholeImage_Parent.transform);
            //ManholeImage_List.Add(img);   


            ClickPoint = GetMouseWorldPosition();          
            if (Physics.Raycast(ClickPoint, transform.up * -1, out hit, Mathf.Infinity, ModelLayer))
            {
                Debug.Log("hit point : " + hit.point + "/" + ClickPoint);
                GameObject obj = Instantiate(Manhole_Prefab, hit.point, Quaternion.identity);

                obj.transform.SetParent(Manhole_Parent.transform);
                Manhole_List.Add(obj);
            }
     

        }
    }

    private void ManholeBtn_Clicked()
    {
        if(!bManholeBtnClicked)
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
    

    //Perspective 카메라 - 마우스 위치 보정
    public Vector3 GetMouseWorldPosition()
    {
        // 마우스 위치에서 시작하는 광선을 생성
        Ray ray = UI_Camera.ScreenPointToRay(Input.mousePosition);

        // 광선이 yPlane(0)과 만나는 점을 계산
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }
        else
        {
            // 기본적으로 평면과의 교차를 계산
            Plane plane = new Plane(Vector3.up, new Vector3(0, UI_Camera.transform.position.y - 10f, 0)); ;
            if (plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
        }

        return Vector3.zero;
    }

}

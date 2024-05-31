using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//맨홀 위치 UI에 띄우는 스크립트
public class Manhole_Finder : MonoBehaviour
{
    private List<Image> img_List = null;

    [SerializeField] private GameObject manholeImage_Parent;
    [SerializeField] private Image manholeImage_Prefab;
    [SerializeField] private Sprite manholeIcon_Sprite;


    private bool bControlManholeShow = false;   //맨홀 확인 한번만 작동하게 하는 변수
    private void Update()
    {
        if(SystemManager.instance.bManholeShow)
        {
            SystemManager.instance.bManholeShow = false;
            //한번만 작동하도록 할것
            MakeManholePoint();
        }
        //else if(!SystemManager.instance.bManholeShow)
        //{     
        //    img_List.Clear();
        //    bControlManholeShow = false;
        //}
    }


    private void MakeManholePoint()
    {
        for(int i = 0; i<SystemManager.instance.ManholeObj_List.Count; i++)
        {
            Vector3 t = SystemManager.instance.UI_Camera.WorldToScreenPoint(SystemManager.instance.ManholeObj_List[i].transform.position);
            Image img = Instantiate(manholeImage_Prefab, t, Quaternion.identity);
            img.transform.SetParent(manholeImage_Parent.transform);
            //img.sprite = manholeIcon_Sprite;
            img_List.Add(img);        
        }
    }
}

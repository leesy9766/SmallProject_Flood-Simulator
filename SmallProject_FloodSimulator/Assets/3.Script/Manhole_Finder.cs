using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//��Ȧ ��ġ UI�� ���� ��ũ��Ʈ
public class Manhole_Finder : MonoBehaviour
{
    private List<Image> img_List = null;

    [SerializeField] private GameObject manholeImage_Parent;
    [SerializeField] private Image manholeImage_Prefab;
    [SerializeField] private Sprite manholeIcon_Sprite;


    private bool bControlManholeShow = false;   //��Ȧ Ȯ�� �ѹ��� �۵��ϰ� �ϴ� ����
    private void Update()
    {
        if(SystemManager.instance.bManholeShow)
        {
            SystemManager.instance.bManholeShow = false;
            //�ѹ��� �۵��ϵ��� �Ұ�
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

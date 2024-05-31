using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Box Collider¿¡ Ãæµ¹µÈ ¸ÇÈ¦ °¹¼ö °ËÃâ
public class Manhole_Checker : MonoBehaviour
{
    [SerializeField] public List<Manhole> Manhole_List;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Manhole"))
        //{
        //    if (!other.GetComponent<Manhole>().isCounted)
        //    {               
        //        Manhole_List.Add(other.GetComponent<Manhole>());
        //    }
            
        //    Debug.Log("¸ÇÈ¦¹ß°ß : " + Manhole_List.Count);
        //}
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Manhole"))
    //    {
    //        Manhole_List.Remove(other.GetComponent<Manhole>());
    //    }


    //}

}

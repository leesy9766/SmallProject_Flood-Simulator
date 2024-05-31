using UnityEngine;

public class Manhole : MonoBehaviour
{
    public float CurrentWaterStrage = 0f;
    public float MaxWaterStarage;
    [SerializeField] public bool isCounted { get; private set; } = false;



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DragBox"))
        {
            other.GetComponent<Manhole_Checker>().Manhole_List.Add(this);
            Debug.Log("발견된 맨홀 갯수 : " + other.GetComponent<Manhole_Checker>().Manhole_List.Count);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("DragBox"))
        {
            isCounted = true;      
        }

       if(other.CompareTag("Building"))
        {
            Debug.Log("설치 불가 위치");
         
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("DragBox"))
        {
            isCounted = false;
            other.GetComponent<Manhole_Checker>().Manhole_List.Remove(this);
            Debug.Log("발견된 맨홀 갯수 : " + other.GetComponent<Manhole_Checker>().Manhole_List.Count);
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            Debug.Log("설치할 수 없는 위치");
            Destroy(gameObject);
        }
    }
}

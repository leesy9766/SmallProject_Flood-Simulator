using UnityEngine;

public class Manhole : MonoBehaviour
{
    [Header("물이 수용되는 양")]
    public float CurrentWaterStrage = 0f;
    public float MaxWaterStarage = 5f;

    [Header("시간당 역류하는 물")]
    public float FloodWaterByTime = 5f; 

    [SerializeField] public bool isCounted { get; private set; } = false;


    private void Start()
    {
        CurrentWaterStrage = 0f;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DragBox"))
        {
            other.GetComponent<Manhole_Checker>().Manhole_List.Add(this);
            Debug.Log("발견된 맨홀 갯수 : " + other.GetComponent<Manhole_Checker>().Manhole_List.Count);
        }


        //to. 미래의 나에게...이것도 고쳐..
        if(other.CompareTag("Manhole"))
        {
            Debug.Log("설치 불가 위치");
            other.GetComponent<Manhole_Checker>().Manhole_List.Remove(this);
            Destroy(this);
           
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

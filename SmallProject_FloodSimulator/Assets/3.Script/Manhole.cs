using UnityEngine;

public class Manhole : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Model"))
        {
            Debug.Log("건물건물!");
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

            if(transform.position.y >= 1f)
            {
                Debug.Log("건물입니다 ^_^ 생성불가!!");
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Model"))
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Debug.Log("건물건물!");
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

            if (transform.position.y >= 1.5f)
            {
                Debug.Log("건물입니다 ^_^ 생성불가!!");
                Destroy(gameObject);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Building"))
        {
            Debug.Log("설치할 수 없는 위치");
        }
    }
}

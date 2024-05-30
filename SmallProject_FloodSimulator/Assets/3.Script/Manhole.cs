using UnityEngine;

public class Manhole : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Model"))
        {
            Debug.Log("�ǹ��ǹ�!");
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

            if(transform.position.y >= 1f)
            {
                Debug.Log("�ǹ��Դϴ� ^_^ �����Ұ�!!");
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Model"))
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Debug.Log("�ǹ��ǹ�!");
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

            if (transform.position.y >= 1.5f)
            {
                Debug.Log("�ǹ��Դϴ� ^_^ �����Ұ�!!");
                Destroy(gameObject);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Building"))
        {
            Debug.Log("��ġ�� �� ���� ��ġ");
        }
    }
}

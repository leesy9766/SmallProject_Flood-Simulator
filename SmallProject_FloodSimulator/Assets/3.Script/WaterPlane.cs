using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlane : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Flooding_co());
    }

    private IEnumerator Flooding_co()
    {
        float time = 0.05f;
        float waterlevel = 0.001f;

        while(true)
        {
            if(!SystemManager.instance.bSimulating)
            {
                yield break;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + waterlevel, transform.position.z);
           
            yield return new WaitForSeconds(time);
        }
    }

}

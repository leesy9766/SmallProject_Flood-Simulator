using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlane : MonoBehaviour
{
    public float floodingSpeed = 0f;
    public float StorageAmount = 0f;
    public int ManholeCount = 0;

    private void Start()
    {
        Init();

        StartCoroutine(Flooding_co());
    }

    private void Init()
    {
        //Debug.Log(StorageAmount);

        //강수량 0이면 스피드 0
        if(SystemManager.instance.RainAmount > 0)
        {
            //그냥 강수량에 따른 순수 침수속도
            floodingSpeed = SystemManager.instance.RainAmount / 10f;
        }
        else
        {
            floodingSpeed = 0f;
        }
        
    }

    private IEnumerator Flooding_co()
    {
        float time = 0.05f;
        float waterlevel = 0.001f * floodingSpeed;

        while(true)
        {
            if(!SystemManager.instance.bSimulating)
            {
                yield break;
            }

            if(StorageAmount >= 0)
            {
                StorageAmount -= time;
            }
            else
            {
                if(ManholeCount > 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + waterlevel * ManholeCount, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + waterlevel, transform.position.z);
                }
              
            }

           
           
            yield return new WaitForSeconds(time);
        }
    }

}

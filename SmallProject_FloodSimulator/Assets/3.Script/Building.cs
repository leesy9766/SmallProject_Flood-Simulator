using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    //침수에 대한 방어력과 높이가 필요한가 ㅎ.ㅎ? 게임인가아

    public float WarningHeight = 0.1f;
    [SerializeField] private float WaterShield;
    private float MaxWaterShield = 0f;
    private Material CurrentMat;

    private bool bisTriggered = false;
     void Start()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && !bisTriggered) 
        {
            bisTriggered = true;
           
            StartCoroutine(WaterPlaneInjectTime_co());
        }    
    }


    private void Init()
    {
        WaterShield = Random.Range(1f, 10f);
        MaxWaterShield = WaterShield;
        CurrentMat = SystemManager.instance.NormalBuilding_M;
    }
    
    private IEnumerator WaterPlaneInjectTime_co()
    {
        float cashing = 0.5f;
        while(true)
        {
            if(!SystemManager.instance.bSimulating)
            {
                yield break;
            }

            if (WaterShield < (MaxWaterShield / 4) && WaterShield > 0)
            {
                CurrentMat = SystemManager.instance.FloodWatch_M;
                GetComponent<MeshRenderer>().material = CurrentMat;
            }
            else if (WaterShield <= 0)
            {
                CurrentMat = SystemManager.instance.FloodWarning_M;
                GetComponent<MeshRenderer>().material = CurrentMat;
                yield break;
            }


                      
            WaterShield -= 0.3f;


            yield return new WaitForSeconds(cashing);
        }
     
    }
}

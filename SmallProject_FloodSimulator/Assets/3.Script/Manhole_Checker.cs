using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Box Collider에 충돌된 맨홀 갯수 검출
public class Manhole_Checker : MonoBehaviour
{
    [SerializeField] public List<Manhole> Manhole_List;

    /*
    1. 시스템매니저의 Dragobj 리스트에서 드래그 콜라이더 오브젝트의 ManholeChecker를 들고와서 각각의 드래그박스의 Manhole_List를 가져온다
    2. 시스템매니저의 이중리스트에 저장한다.
    3. 드래그박스 리스트와 WaterPlane 리스트의 순서(i)는 같다
    4. 맨홀에서 시간당 토해내는 물의 양을 계산하는 식을 생각해보자..   
        4-1. 비가옴
        4-2. 맨홀이 역류를함
        4-2. 시간당 몇톤을 뱉느냐에 따른 속도 조절

    입력받아야할 값 : 맨홀의 시간당 뱉는 물 양
    출력되어야 할 것 : 침수의 속도 값

    강수량이 n일 때 n에대해 수용불가능한 값을 가진 맨홀만 역류시키기 ->침수속도 upup
     
     */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSetter : MonoBehaviour
{
    public GameObject SuccessPrefab;
    public GameObject FailPrefab;

    public void SetBombMark(Vector3 world, bool isSuccess) 
    {
        GameObject prefab = isSuccess ? SuccessPrefab : FailPrefab;
        GameObject inst = Instantiate(prefab, transform);

        world.y = transform.position.y;
        inst.transform.position = world;
    }

    public void ResetBombMark() 
    {
        while (transform.childCount > 0) //삭제같은 경우는 Destroy에 시간이 오래 걸려서 자식을 제거할떼는 일단 자식에서 빼내고 제거한다. 그리고 같은이유로 for foreach 보다 while이 안정성이 높다
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }
}

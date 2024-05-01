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
        while (transform.childCount > 0) //�������� ���� Destroy�� �ð��� ���� �ɷ��� �ڽ��� �����Ҷ��� �ϴ� �ڽĿ��� ������ �����Ѵ�. �׸��� ���������� for foreach ���� while�� �������� ����
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }
}

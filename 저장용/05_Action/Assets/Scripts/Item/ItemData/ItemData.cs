using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Ƽ���� ����� �� �ִ� �޴�, ��Ŭ������ ������ �޴��ۼ� ���ϳ����� ����Ʈ ����, �޴������� ��Ŭ������ �޴������� �޴��̸�  
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order =0)]
public class ItemData : ScriptableObject        //������ �� ������ ������ �����ϴ� ��ũ���ͺ� ������Ʈ
{
    [Header("������ �⺻ ����")]
    public ItemCode code;
    public string itemName = "������";
    public string itemDescription = "����";
    public Sprite itemIcon;
    public uint price = 0;              //uint = unsignedInt ��ȣ���� ��Ʈ
    public uint maxStackCount = 1;
    public GameObject modelPrefab;
}

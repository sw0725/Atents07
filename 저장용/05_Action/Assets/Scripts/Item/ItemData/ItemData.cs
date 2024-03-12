using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유니티에서 사용할 수 있는 메뉴, 우클릭에서 나오는 메뉴작성 파일네임은 디폴트 네임, 메뉴네임은 우클릭시의 메뉴구성및 메뉴이름  
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order =0)]
public class ItemData : ScriptableObject        //아이템 한 종류의 정보를 저장하는 스크립터블 오브젝트
{
    [Header("아이템 기본 정보")]
    public ItemCode code;
    public string itemName = "아이템";
    public string itemDescription = "설명";
    public Sprite itemIcon;
    public uint price = 0;              //uint = unsignedInt 부호없는 인트
    public uint maxStackCount = 1;
    public GameObject modelPrefab;
}

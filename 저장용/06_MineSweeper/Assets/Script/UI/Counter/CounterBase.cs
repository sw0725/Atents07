using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ImageNumber))]     //필수적 스크립트 표시 없으면 자동 채워줌
public class CounterBase : MonoBehaviour
{
    ImageNumber imageNumber;

    protected virtual void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }
    protected void Refreash(int count)
    {
        imageNumber.Number = count;
    }
}

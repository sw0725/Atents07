using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ImageNumber))]     //�ʼ��� ��ũ��Ʈ ǥ�� ������ �ڵ� ä����
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

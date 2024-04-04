using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : MonoBehaviour
{
    ImageNumber imageNumber;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        GameManager.Instance.onFlagCountChange += Refreash;
        Refreash(GameManager.Instance.FlagCount);
    }

    private void Refreash(int count)
    {
        imageNumber.Number = count;
    }
}

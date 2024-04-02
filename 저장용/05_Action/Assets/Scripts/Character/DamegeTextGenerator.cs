using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeTextGenerator : MonoBehaviour
{
    private void Start()
    {
        IBattler battler = GetComponentInParent<IBattler>();
        battler.onHit += DamegeTextGenerate;
    }

    void DamegeTextGenerate(int damege) 
    {
        Factory.Instance.GetDamegeText(damege, transform.position);
    }
}

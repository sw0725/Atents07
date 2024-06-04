using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    TextMeshProUGUI current;
    TextMeshProUGUI max;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        current = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(2);
        max = c.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.AddBulletCountChangeDelegate(OnBulletCountChange);
        player.onGunChange += OnGunChange;
    }

    void OnBulletCountChange(int count) 
    {
        current.text = count.ToString();
    }

    void OnGunChange(GunBase gun) 
    {
        max.text = gun.clipSize.ToString();
    }
}

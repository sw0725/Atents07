using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    Player player;
    Vector3 pos = new Vector3(0, 20, 0);

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        pos.x = player.transform.position.x;
        pos.z = player.transform.position.z;
        transform.position = pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    const int HeightCount = 3;
    const int WidthCount = 3;
    const float mapHeightSize = 20.0f;
    const float mapWidthSize = 20.0f;
                                                             //9�� ������ �߽��� ���� �Ʒ���
    readonly Vector2 wordOrigine = new Vector2(-mapWidthSize * WidthCount* 0.5f, -mapHeightSize * HeightCount * 0.5f);       
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkBanner : MonoBehaviour
{
    Transform background;
    Transform letter1;
    Transform letter2;

    private void Awake()
    {
        background = transform.GetChild(0);
        letter1 = transform.GetChild(1);
        letter2 = transform.GetChild(2);
    }

    private void Start()
    {
        Ship ship = GetComponentInParent<Ship>();
        ship.onSink += Open;
        ship.onDeploy += (_) => Close();

        Close();
    }

    void Open(Ship ship)
    {
        transform.rotation = Quaternion.Euler(0, (int)ship.Direction * 90, 0);

        background.localScale = new Vector3(1, ship.Size, 1);
        background.localPosition = new Vector3(0, 0, 0.5f + (-0.5f * ship.Size));

        letter1.localRotation = Quaternion.Euler(90, 0, (int)ship.Direction * 90);
        letter2.localRotation = Quaternion.Euler(90, 0, (int)ship.Direction * 90);

        Vector3 dir = Vector3.zero;

        switch (ship.Direction)
        {
            case ShipDirection.North:
                dir.x = 0;
                dir.y = 0;
                dir.z = -1;
                break;
            case ShipDirection.East:
                dir.x = -1;
                dir.y = 0;
                dir.z = 0;
                break;
            case ShipDirection.South:
                dir.x = 0;
                dir.y = 0;
                dir.z = 1;
                break;
            case ShipDirection.West:
                dir.x = 1;
                dir.y = 0;
                dir.z = 0;
                break;
        }

        letter1.localPosition = Vector3.up * 0.01f;
        letter2.localPosition = Vector3.up * 0.01f;

        float diff = (ship.Size - 2) * 0.25f;
        Vector3 letter1Pos = letter1.position + dir * diff;
        Vector3 letter2Pos = letter1.position + dir * (ship.Size - 1 - diff);

        if (ship.Direction == ShipDirection.East || ship.Direction == ShipDirection.South) 
        {
            (letter1Pos, letter2Pos) = (letter2Pos, letter1Pos);
        }

        letter1.position = letter1Pos;
        letter2.position = letter2Pos;

        gameObject.SetActive(true);
    }

    void Close() 
    {
        gameObject.SetActive(false);
    }
}

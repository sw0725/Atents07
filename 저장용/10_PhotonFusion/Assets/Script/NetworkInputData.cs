using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte MouseButtonLeft = 1;  //��ư�� ���� byte������ - �Ұ���
    public const byte MouseButtonRight = 2;  //��ư�� ���� byte������ - �Ұ���

    public NetworkButtons buttons;          //��ư���� �Է� ��Ȳ�� ����
    public Vector3 direction;
}

using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte MouseButtonLeft = 1;  //버튼의 종류 byte값에는 - 불가능
    public const byte MouseButtonRight = 2;  //버튼의 종류 byte값에는 - 불가능

    public NetworkButtons buttons;          //버튼들의 입력 상황을 받음
    public Vector3 direction;
}

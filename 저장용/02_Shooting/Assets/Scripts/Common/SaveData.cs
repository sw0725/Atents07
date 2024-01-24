using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//제이슨 유틸리티에서 사용하기위해 반드시 직렬화가능 클래스여야함
[Serializable]
public class SaveData           //저장용 클래스는 일반클래스여야함 => 상속x
{
    public string[] rankerName;
    public int[] highScore;
}

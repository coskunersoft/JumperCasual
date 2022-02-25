using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BotPointJumpValue",menuName = "Coskunerov/BotPointJumpValue")]
public class BPJumpValue : BotPointValue
{
    public float Min;
    public float Max;
    public float ReadyTime;
    public float GetValue => Random.Range(Min, Max);
}

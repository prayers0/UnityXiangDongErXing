using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : CharacterViewBase
{
    public override void Init(Action onFootStep, Action onSkillStop)
    {
        base.Init(onFootStep, onSkillStop);
        GetComponent<SpriteRenderer>().sortingOrder = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }
}

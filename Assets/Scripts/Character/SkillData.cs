using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SkillData
{
    [Header("共享")]
    public string animationName;
    public float attackValueMultiply;
    public bool canMove;
    public float moveSpeedMultiply;
    public AudioClip releaseAudio;
    public AudioClip hitClip;
    [Header("玩家")]
    public bool releaseOnJump;
    public bool fanFilp;
    public float moveSpeedMultiplyOnJump;
    public HitData hitData;
    [Header("敌人")]
    public Vector2 attackRange;

    [Serializable]
    public class HitData
    {
        public float knockbackSpeed;//击退速度
        public float knockbackTime;//击退速度
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SkillData
{
    [Header("����")]
    public string animationName;
    public float attackValueMultiply;
    public bool canMove;
    public float moveSpeedMultiply;
    public AudioClip releaseAudio;
    public AudioClip hitClip;
    [Header("���")]
    public bool releaseOnJump;
    public bool fanFilp;
    public float moveSpeedMultiplyOnJump;
    public HitData hitData;
    [Header("����")]
    public Vector2 attackRange;

    [Serializable]
    public class HitData
    {
        public float knockbackSpeed;//�����ٶ�
        public float knockbackTime;//�����ٶ�
    }
}

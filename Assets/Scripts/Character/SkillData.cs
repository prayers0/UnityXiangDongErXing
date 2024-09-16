using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SkillData
{
    public string animationName;
    public float attackValue;
    public bool releaseOnJump;
    public bool canMove;
    public bool fanFilp;
    public float moveSpeedMultiply;
    public float moveSpeedMultiplyOnJump;
    public AudioClip releaseAudio;
}

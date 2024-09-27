using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterControllerBase<PlayerView>
{
    private int currentSkillIndex=0;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    private void Update()
    {
        if (CanJump()&&Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        float h = Input.GetAxisRaw("Horizontal");
        if(SkillState())//技能状态下 
        {
            //如果当前在空中并且这个技能支持控制则使用空中的速度，否则使用常规技能的移动速度
            if (currentSkill.canMove)
            {
                float moveSpeedMultipy = JumpState() && currentSkill.releaseOnJump ? currentSkill
                .moveSpeedMultiplyOnJump : currentSkill.moveSpeedMultiplyOnJump;

                Move(h * moveSpeedMultipy, currentSkill.fanFilp, false);
            }
            else StopMove();
        }
        else//常规移动
        {
            if(CanMove()) Move(h);
            else StopMove();
            //技能释放
            if (Input.GetMouseButton(0) && CanReleaseSkill(currentSkillIndex))
            {
                ReleaseSkill(currentSkillIndex);
                currentSkillIndex += 1;
                if (currentSkillIndex >= skillDatas.Length)
                {
                    currentSkillIndex = 0;
                }
            }
        }
    }

    protected override void Die()
    {
        
    }
}

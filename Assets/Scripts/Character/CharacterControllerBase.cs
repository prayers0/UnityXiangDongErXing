using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase<V> : MonoBehaviour,IDamageable where V : CharacterViewBase
{
    [SerializeField] protected V view;
    [SerializeField] protected new Rigidbody2D rigidbody;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected SkillData[] skillDatas;

    [Header("音效资源")]
    [SerializeField] protected AudioClip[] footStepClips;

    protected SkillData currentSkill;

    public virtual void Init()
    {
        view.Init(OnFootStep, OnSkillStop);
        weapon.Init(OnHit);
    }

    protected bool JumpState()
    {
        return rigidbody.velocity.y != 0;
    }

    public bool CanJump()
    {
        return !JumpState()&&!SkillState();
    }

    public void Jump()
    {
        rigidbody.velocity = Vector2.up * jumpPower;
    }

    protected bool CanMove()
    {
        return !SkillState();//释放技能
    }

    public void Move(float h,bool filp=true,bool autoAnimation = true)
    {
        bool runState = h != 0;
        rigidbody.velocity = new Vector3(h * moveSpeed, rigidbody.velocity.y, 0);
        if (filp && runState) view.SetDir(h > 0);
        if (autoAnimation)
        {
            if(JumpState())
            {
                if (rigidbody.velocity.y > 1f)
                {
                    view.PlayerAnimation("Rise");
                }
                else if (rigidbody.velocity.y < -1f)
                {
                    view.PlayerAnimation("Fall");
                }
            }
            else
            {
                view.PlayerAnimation(runState ? "Run" : "Idle");
            }
        }
    }

    protected void StopMove()
    {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
    }

    protected void OnFootStep()
    {
        if (footStepClips.Length == 0) return;
        AudioManager.Instance.PlayerAudio(footStepClips[Random.Range(0, footStepClips.Length)],0.3f);
    }


    protected bool SkillState()
    {
        return currentSkill != null;
    }

    protected bool CanReleaseSkill(int skillIndex)
    {
        if (SkillState()) return false;
        if (skillDatas[skillIndex].releaseOnJump)
        {
            return true;
        }
        else
        {
            return !JumpState();
        }
    }

    protected void ReleaseSkill(int skillIndex)
    {
        currentSkill = skillDatas[skillIndex];
        weapon.SetSkill(skillIndex);//设置武器
        view.PlayerAnimation(currentSkill.animationName);
        if (currentSkill.releaseAudio != null)
        {
            AudioManager.Instance.PlayerAudio(currentSkill.releaseAudio);
        }
    }

    public void Hurt(float damage)
    {
    }

    private void OnHit(IDamageable damageable, int skillIndex)
    {
        damageable.Hurt(skillDatas[skillIndex].attackValue);
    }

    private void OnSkillStop()
    {
        currentSkill = null;
        weapon.StopCheck();
    }

}

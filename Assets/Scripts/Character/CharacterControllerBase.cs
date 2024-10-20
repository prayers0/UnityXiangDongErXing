using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController 
{
    Transform transform { get; }
}
public abstract class CharacterControllerBase<V> : MonoBehaviour,ICharacterController,IDamageable where V : CharacterViewBase
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
    protected float currentHP;

    protected float CurrentHP 
    {
        get => currentHP;
        set
        {
            if (currentHP == value) return;
            currentHP = value;
            OnHPChanged(currentHP);
            if (currentHP <= 0)
            {
                currentHP = 0;
                Die();
            }
        } 
    }

    protected virtual void OnHPChanged(float newHp)
    {

    }

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

    protected virtual void Move(float h,bool filp=true,bool autoAnimation = true)
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

    protected virtual void OnFootStep()
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
        view.PlayerAnimation(currentSkill.animationName,true);
        if (currentSkill.releaseAudio != null)
        {
            AudioManager.Instance.PlayerAudio(currentSkill.releaseAudio);
        }
    }

    public void Hurt(float damage)
    {
    }

    //打别人
    private void OnHit(IDamageable damageable, int skillIndex)
    {
        SkillData skillData = skillDatas[skillIndex];
        damageable.Hurt(skillData.attackValue,this,skillData.hitData);
        if (skillData.hitData != null) AudioManager.Instance.PlayerAudio(skillData.hitClip);
    }

    private void OnSkillStop()
    {
        currentSkill = null;
        weapon.StopCheck();
    }
    //被打
    public virtual void Hurt(float damage, ICharacterController source, SkillData.HitData hitData)
    {
        view.PlayHurtAnimation();
        CurrentHP -= damage;
    }

    //死亡
    protected abstract void Die();

}

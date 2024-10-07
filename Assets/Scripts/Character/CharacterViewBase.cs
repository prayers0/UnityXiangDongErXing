using System;
using System.Collections;
using UnityEngine;


public class CharacterViewBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected Weapon weapon;
    protected string currentAnimationName;
    protected Vector3 lertDirScale = new Vector3(-1, 1, 1);
    protected Vector3 rightDirScale = new Vector3(1, 1, 1);

    public virtual void Init(Action onFootStep,Action onSkillStop)
    {
        this.onFootStep = onFootStep;
        this.onSkillStop = onSkillStop;
    }

    public void PlayerAnimation(string animationName, bool refresh = false)
    {
        if (animationName == currentAnimationName && !refresh) return;
        currentAnimationName = animationName;
        animator.Play(animationName, 0,0);
    }

    public void PlayHurtAnimation()
    {
        animator.Play("Hurt", 1,0);
    }

    public void SetDir(bool right)
    {
        transform.localScale = right ? rightDirScale : lertDirScale;
    }

    public bool IsRight()
    {
        return transform.lossyScale == rightDirScale;
    }

    #region 动画事件
    private Action onFootStep;
    private Action onSkillStop;
    private void FootStep()
    {
        onFootStep?.Invoke();
    }

    private void WeaponStartCheck()
    {
        weapon.StartCheck();
    }

    private void WeaponStopCheck()
    {
        weapon.StopCheck();
    }

    private void SkillStop()
    {
        onSkillStop?.Invoke();
    }
    #endregion


}

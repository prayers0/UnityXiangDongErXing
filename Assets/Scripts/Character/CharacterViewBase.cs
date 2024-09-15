using System.Collections;
using UnityEngine;


public class CharacterViewBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected string currentAnimationName;
    protected Vector3 lertDirScale = new Vector3(-1, 1, 1);
    protected Vector3 rightDirScale = new Vector3(1, 1, 1);

    public virtual void Init()
    {

    }

    public void PlayerAnimation(string animationName, bool refresh = false)
    {
        if (animationName == currentAnimationName && !refresh) return;
        currentAnimationName = animationName;
        animator.Play(animationName, 0);
    }

    public void SetDir(bool right)
    {
        transform.localScale = right ? rightDirScale : lertDirScale;
    }
}

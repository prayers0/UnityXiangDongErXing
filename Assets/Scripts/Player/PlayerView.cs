using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string currentAnimationName;

    public void Init()
    {

    }

    public void PlayerAnimation(string animationName,bool refresh = false)
    {
        if (animationName == currentAnimationName && !refresh) return;
        currentAnimationName=animationName;
        animator.Play(animationName, 0);
    }

    private Vector3 lertDirScale = new Vector3(-1, 1, 1);
    private Vector3 rightDirScale=new Vector3(1, 1, 1);

    public void SetDir(bool right)
    {
        transform.localScale=right?rightDirScale:lertDirScale;
    }
}

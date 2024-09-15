using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase<V> : MonoBehaviour where V : CharacterViewBase
{
    [SerializeField] protected V view;
    [SerializeField] protected new Rigidbody2D rigidbody;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpPower;

    public virtual void Init()
    {
        view.Init();
    }

    public bool CanJump()
    {
        return rigidbody.velocity.y == 0;
    }

    public void Jump()
    {
        rigidbody.velocity = Vector2.up * jumpPower;
    }

    public void Move(float h,bool filp=true,bool autoAnimation = true)
    {
        bool jumpState = rigidbody.velocity.y != 0;
        bool runState = h != 0;
        rigidbody.velocity = new Vector3(h * moveSpeed, rigidbody.velocity.y, 0);
        if (filp && runState) view.SetDir(h > 0);
        if (autoAnimation)
        {
            if(jumpState)
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterControllerBase<PlayerView>
{
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

        Move(h);
    }
}

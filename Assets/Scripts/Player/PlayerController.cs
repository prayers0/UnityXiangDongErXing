using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField]public float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private PlayerView playerView;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        playerView.Init();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (rigidbody.velocity.y==0&&Input.GetKey(KeyCode.Space))
        {
            rigidbody.velocity = Vector2.up*jumpPower;
        }
        bool jumpState = rigidbody.velocity.y != 0;
        if (jumpState&&rigidbody.velocity.y > 1f)
        {
            playerView.PlayerAnimation("Rise");
        }
        else if(jumpState && rigidbody.velocity.y < -1f)
        {
            playerView.PlayerAnimation("Fall");
        }
        bool runState = h != 0;
        rigidbody.velocity = new Vector3(h * moveSpeed, rigidbody.velocity.y, 0);

        if (runState)
        {
            //transform.Translate(new Vector3(h * Time.deltaTime * moveSpeed, 0, 0));
            playerView.SetDir(h > 0);
        }
        if(!jumpState) playerView.PlayerAnimation(runState ? "Run" : "Idle");

        //rigidbody.velocity = new Vector2(h * Time.deltaTime * moveSpeed, rigidbody.velocity.y);
    }
}

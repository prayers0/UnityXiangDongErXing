using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField]public float moveSpeed;
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
        float h = Input.GetAxis("Horizontal");
        bool runState = h != 0;
        if (runState)
        {
            transform.Translate(new Vector3(h * Time.deltaTime * moveSpeed, 0, 0));
            playerView.SetDir(h > 0);
        }

        playerView.PlayerAnimation(runState ? "Run" : "Idle");
        //rigidbody.velocity = new Vector2(h * Time.deltaTime * moveSpeed, rigidbody.velocity.y);
    }
}

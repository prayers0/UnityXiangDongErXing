using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(h * Time.deltaTime * moveSpeed, 0, 0));
    }
}

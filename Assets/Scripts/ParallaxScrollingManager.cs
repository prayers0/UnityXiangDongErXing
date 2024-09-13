using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrollingManager : MonoBehaviour
{
    public ParallaxScrollingLayer[] layers;
    public float screenWidth=40;
    public Transform target;
    private float playerPosX => target.position.x;

    private void LateUpdate()
    {
        foreach(ParallaxScrollingLayer layer in layers)
        {
            float targetPosX = playerPosX * layer.distance - screenWidth / 2f;
            Vector3 pos=layer.transform.position;
            pos.x = targetPosX;
            layer.transform.position = pos;
            layer.spriteRenderer.size = new Vector2(playerPosX + screenWidth, layer.spriteRenderer.size.y);
        }
    }
}

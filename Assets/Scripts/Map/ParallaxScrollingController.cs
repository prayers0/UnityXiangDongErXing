using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrollingController : MonoBehaviour
{
    [SerializeField]public ParallaxScrollingLayer[] layers;
    private float screenWidth;

    public void Init(float screenWidth)
    {
        this.screenWidth = screenWidth;
    }

    public void UpdateLyayrs(float newPlayerPosX)
    {
        foreach (ParallaxScrollingLayer layer in layers)
        {
            float targetPosX = newPlayerPosX * layer.distance - screenWidth / 2f;
            Vector3 pos = layer.transform.position;
            pos.x = targetPosX;
            layer.transform.position = pos;
            layer.spriteRenderer.size = new Vector2(newPlayerPosX + screenWidth, layer.spriteRenderer.size.y);
        }
    }

    
}

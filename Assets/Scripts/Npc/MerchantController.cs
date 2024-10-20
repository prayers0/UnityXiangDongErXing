using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : NPCBase
{
    public new Animation hintAnimation;
    public Vector2Int itemCountRange;
    private List<ItemConfigBase> items;

    public override void Init(System.Random mapChunkRandom)
    {
        //Ëæ»úÉÌÆ·
        int count = mapChunkRandom.Next(itemCountRange.x, itemCountRange.y+1);
        items = new List<ItemConfigBase>(count);
        for(int i = 0; i < count; i++)
        {
            items.Add(ResManager.Instance.GetRandomItemConfig(mapChunkRandom));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hintAnimation.Play("Door_Hint");
        PlayerController.Instance.OnMerchantStay(items);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hintAnimation.Play("Door_Idle");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    public new Animation hintAnimation;

    public void Init()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hintAnimation.Play("Door_Hint");
        //PlayerController.Instance.OnDoorStay(MapController.current.GetCellCoord(transform.position.x)
        //    , doorConfig.isEntrance);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hintAnimation.Play("Door_Idle");
    }
}

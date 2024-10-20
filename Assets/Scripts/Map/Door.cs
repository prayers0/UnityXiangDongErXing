using UnityEngine;

public class Door : MonoBehaviour
{
    public new Animation hintAnimation;
    public MapDungeonDoorConfig doorConfig;

    public void Init(MapDungeonDoorConfig doorConfig)
    {
        this.doorConfig = doorConfig;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hintAnimation.Play("Door_Hint");
        PlayerController.Instance.OnDoorStay(MapController.current.GetCellCoord(transform.position.x)
            ,doorConfig.isEntrance);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hintAnimation.Play("Door_Idle");
    }
}

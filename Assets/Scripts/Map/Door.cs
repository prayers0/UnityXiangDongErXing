using UnityEngine;

public class Door : MonoBehaviour
{
    public new Animation animation;
    public MapDungeonDoorConfig doorConfig;

    public void Init(MapDungeonDoorConfig doorConfig)
    {
        this.doorConfig = doorConfig;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        animation.Play("Door_Hint");
        PlayerController.Instance.OnDoorStay(MapController.current.GetCellCoord(transform.position.x)
            ,doorConfig.isEntrance);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animation.Play("Door_Idle");
    }
}

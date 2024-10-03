using UnityEngine;

public class Door : MonoBehaviour
{
    public new Animation animation;

    private void OnTriggerStay2D(Collider2D collision)
    {
        animation.Play("Door_Hint");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animation.Play("Door_Idle");
    }
}

using UnityEngine;

public class CoinController : MonoBehaviour, IDropObject
{
    private int mapChunkCoord;
    public new Rigidbody2D rigidbody2D;
    public void Init(int mapChunkCoord)
    {
        this.mapChunkCoord = mapChunkCoord;
        rigidbody2D.velocity = Vector2.up * Random.Range(3, 5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && rigidbody2D.velocity.y < 0)
        {
            GameSceneManager.Instance.AddCoin(10);
            AudioManager.Instance.PlayerAudio();
            Destroy(gameObject);
        }
    }
}

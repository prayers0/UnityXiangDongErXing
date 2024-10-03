using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private void  Start()
    {
        //ºÚÄ»ÕÚµ²
        //UIManager.Instance.ShowWindow<UI_BlackCanvasDropWindow>();
        PlayerController.Instance.transform.position = GameManager.Instance.gameData.playerPos.ToVectr3();
        PlayerController.Instance.Init();
        MapController.current.Init(GameManager.Instance.gameData.mapSeed, PlayerController.Instance);
        MapController.current.UpdateMapChunk();
        //UIManager.Instance.CloseWindow<UI_BlackCanvasDropWindow>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ShowWindow<UI_GameScenePopUpWindow>();
        }
    }
}

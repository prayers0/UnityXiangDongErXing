using UnityEngine;

public class GameOverSceneManager : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.ShowWindow<UI_GameOverWindow>();
    }
}

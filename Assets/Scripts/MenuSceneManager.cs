using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.ShowWindow<UI_MenuSceneMainWindow>();
    }
}

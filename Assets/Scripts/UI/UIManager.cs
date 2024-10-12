using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Transform normalLayer;
    public Transform dragLayer;
    public List<GameObject> windowPrefabfabList;//约定窗口预制体的名称与其管理脚本的名称完全一致
    private Dictionary<string,UI_WindowBase> windowCache=new Dictionary<string, UI_WindowBase>();

    private void Awake()
    {
        //if (Instance != null)//场景已经有一个Manager
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public T ShowWindow<T>() where T : UI_WindowBase
    {
        string windowName=typeof(T).Name;
        //查找当前是否存在这个窗口，不存在则实例化
        if(!windowCache.TryGetValue(windowName,out UI_WindowBase window))
        {
            GameObject prefab=GetUIPrefab(windowName);
            window = GameObject.Instantiate(prefab, normalLayer).GetComponent<T>();
            window.OnShow();
            windowCache.Add(windowName, window);
        }
        return (T)window;
    }

    public void CloseWindow<T>() where T : UI_WindowBase
    {
        string windowName = typeof(T).Name;
        //找到在关闭
        if (windowCache.Remove(windowName, out UI_WindowBase window))
        {
            window.OnClose();
            Destroy(window.gameObject);
        }
    }

    //窗口如果是关闭的则打开
    //若以及开启则关闭
    //bool 返回值，代表的时最终开启状态
    //out window，如果最终为开启状态则为窗口的引用，否则为null
    public bool ToggleWindow<T>(out T window) where T : UI_WindowBase
    {
        string windowName = typeof(T).Name;
        window = null;
        //找到在关闭
        if (windowCache.Remove(windowName, out UI_WindowBase tempWindow))
        {
            tempWindow.OnClose();
            Destroy(tempWindow.gameObject);
            return false;
        }
        //没找到则开启
        else
        {
            GameObject prefab = GetUIPrefab(windowName);
            window = GameObject.Instantiate(prefab, normalLayer).GetComponent<T>();
            window.OnShow();
            windowCache.Add(windowName, window);
            return true;
        }

    }

    public void CloseAllWindow()
    {
        foreach(UI_WindowBase window in windowCache.Values)
        {
            window.OnClose();
            Destroy(window.gameObject);
        }
        windowCache.Clear();
    }

    private GameObject GetUIPrefab(string prefabName)
    {
        for(int i = 0; i < windowPrefabfabList.Count; i++)
        {
            if (windowPrefabfabList[i].name == prefabName)
            {
                return windowPrefabfabList[i];
            }
        }
        return null;
    }
}
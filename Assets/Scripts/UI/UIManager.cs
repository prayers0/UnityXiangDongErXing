using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Transform normalLayer;
    public Transform dragLayer;
    public List<GameObject> windowPrefabfabList;//Լ������Ԥ����������������ű���������ȫһ��
    private Dictionary<string,UI_WindowBase> windowCache=new Dictionary<string, UI_WindowBase>();

    private void Awake()
    {
        //if (Instance != null)//�����Ѿ���һ��Manager
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
        //���ҵ�ǰ�Ƿ����������ڣ���������ʵ����
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
        //�ҵ��ڹر�
        if (windowCache.Remove(windowName, out UI_WindowBase window))
        {
            window.OnClose();
            Destroy(window.gameObject);
        }
    }

    //��������ǹرյ����
    //���Լ�������ر�
    //bool ����ֵ�������ʱ���տ���״̬
    //out window���������Ϊ����״̬��Ϊ���ڵ����ã�����Ϊnull
    public bool ToggleWindow<T>(out T window) where T : UI_WindowBase
    {
        string windowName = typeof(T).Name;
        window = null;
        //�ҵ��ڹر�
        if (windowCache.Remove(windowName, out UI_WindowBase tempWindow))
        {
            tempWindow.OnClose();
            Destroy(tempWindow.gameObject);
            return false;
        }
        //û�ҵ�����
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
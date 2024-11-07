using System;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    public GameObject mainMapPrefab;
    public GameObject dungeonMapPrefab;
    private GameData gameData => GameManager.Instance.gameData;
    private UI_GameMainWindow mainWindow;
    private void Awake()
    {
        Instance = this;
    }
    private void  Start()
    {
        mainWindow=UIManager.Instance.ShowWindow<UI_GameMainWindow>();
        mainWindow.SetCoin(gameData.coinCount);
        mainWindow.SetHp(gameData.playerHp / ResManager.Instance.PlayerMaxHp);
        PlayerController.Instance.Init();
        PlayerController.Instance.InitHP(gameData.playerHp);
        if (gameData.onMainMap) LoadMainMap();
        else LoadDungeonMap();
        //��Ļ�ڵ�
        //UIManager.Instance.ShowWindow<UI_BlackCanvasDropWindow>();
        
        //UIManager.Instance.CloseWindow<UI_BlackCanvasDropWindow>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ToggleWindow<UI_GameScenePopUpWindow>(out _);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if(UIManager.Instance.ToggleWindow<UI_BagWindow>(out UI_BagWindow bagWindow))
            {
                bagWindow.Show(gameData.bagData);
            }
        }
    }

    public void OpenBagWindow()
    {
        if(!UIManager.Instance.TryGetWindow(out UI_BagWindow bagWindow))
        {
            UIManager.Instance.ShowWindow<UI_BagWindow>().Show(gameData.bagData);
        }
    }

    private void BlackCackDrop()
    {
        UIManager.Instance.ShowWindow<UI_BlackCanvasDropWindow>().Fade(2);
    }

    public void LoadMainMap()
    {
        BlackCackDrop();
        gameData.onMainMap = true;
        if (MapController.HaveMap)
        {
            MapController.current.Destroy();
        }
        
        MapController mapController = Instantiate(mainMapPrefab).GetComponent<MapController>();
        PlayerController.Instance.transform.position = GetMainMapPlayerPosition(mapController);
        mapController.Init(gameData.mapSeed, PlayerController.Instance);
        mapController.UpdateMapChunk();
    }


    private void LoadDungeonMap()
    {
        BlackCackDrop();
        gameData.onMainMap = false;
        if (MapController.HaveMap)
        {
            MapController.current.Destroy();
        }
        
        MapController mapController = Instantiate(dungeonMapPrefab).GetComponent<MapController>();
        PlayerController.Instance.transform.position = GetDungeonMapPlayerPosition(mapController);
        int seed = (gameData.mapSeed + 100) / 2 + gameData.dungeonCoord;
        mapController.Init(seed, PlayerController.Instance);
        mapController.UpdateMapChunk();
    }

    public void EnterDungeonMap(int doorCoord)
    {
        gameData.onMainMap = false;
        if (MapController.HaveMap)
        {
            MapController.current.Destroy();
        }
        MapController mapController=Instantiate(dungeonMapPrefab).GetComponent<MapController>();
        gameData.playerDungeonMapPos = default;
        PlayerController.Instance.transform.position=GetDungeonMapPlayerPosition(mapController);
        int seed = (gameData.mapSeed + 100) / 2 + doorCoord;
        gameData.dungeonCoord = seed;
        mapController.Init(seed,PlayerController.Instance);
        mapController.UpdateMapChunk();
    }

    private Vector3 GetMainMapPlayerPosition(MapController mapController)
    {
        Vector3 pos = gameData.playerMainPos.ToVectr3();
        if (pos == default)
        {
            pos = mapController.GetPlayerDefaultPosition();
            gameData.playerMainPos = new SVector3(pos);
        }
        return pos;
    }

    private Vector3 GetDungeonMapPlayerPosition(MapController mapController)
    {
        //Vector3 pos = default;
        Vector3 pos = gameData.playerDungeonMapPos.ToVectr3();
        if (pos == default)
        {
            pos = mapController.GetPlayerDefaultPosition();
            gameData.playerDungeonMapPos = new SVector3(pos);
        }
        return pos;
    }

    public void UpdatePlayerPositionData(Vector3 position)
    {
        if (gameData.onMainMap)
        {
            gameData.playerMainPos=new SVector3(position);
        }
        else
        {
            gameData.playerDungeonMapPos = new SVector3(position);
        }
    }

    public void SetCoin(int count)
    {
        gameData.coinCount = count;
        mainWindow.SetCoin(gameData.coinCount);
    }

    public void SetHp(float hp)
    {
        gameData.playerHp = hp;
        mainWindow.SetHp(gameData.playerHp/ResManager.Instance.PlayerMaxHp);
    }

    public void BuyItem(ItemConfigBase itemConfig, int index)
    {
        //�ж��Ƿ����㹻�Ľ�ң���������н����˸������
        if (gameData.coinCount<itemConfig.price)
        {
            mainWindow.CoinFlash();
            return;
        }
        //�ж��Ƿ����㹻�Ľ��
        //������Ʒ
        //��������ռ��һ���ո���
        if (itemConfig is WeaponConfig)
        {
            if (gameData.bagData.items[index] != null) return;
            //����һ����ͬ�����ݱ��浽�����й�
            gameData.bagData.items[index] = itemConfig.GetDefaultData().Copy();
        }
        //����ƷҪ���ȿ��Ƕѵ�
        else if (itemConfig is ConsunableConfig)
        {
            //�����ҵ��Ѿ��е�ͬ����Ʒ���е���
            if (gameData.bagData.TryGetItem(itemConfig.name, out int existIndex))
            {
                ConsumableData consumableData = (ConsumableData)gameData.bagData.items[existIndex];
                consumableData.count += ((ConsumableData)itemConfig.GetDefaultData()).count;
                index = existIndex;
            }
            //Ҫ��һ����λ
            else
            {
                if (gameData.bagData.items[index] != null) return;
                gameData.bagData.items[index] = itemConfig.GetDefaultData().Copy();
            }
        }
        //ͬ�����
        SetCoin(gameData.coinCount - itemConfig.price);
        //ͬ��UI��Ʒ����
        if (UIManager.Instance.TryGetWindow(out UI_BagWindow bagWindow))
        {
            bagWindow.UpdateSlot(index, gameData.bagData.items[index]);
        }

    }

    public void SellItem(int index)
    {
        ItemDataBase itemData = gameData.bagData.items[index];
        ItemConfigBase itemConfig = ResManager.Instance.GetItemConfig(itemData.id);
        //���ӽ��
        if(itemData is WeaponData)
        {
            SetCoin(gameData.coinCount + itemConfig.price / 2);
        }
        else if(itemData is ConsumableData)
        {
            int count = ((ConsumableData)itemData).count;
            SetCoin((gameData.coinCount + itemConfig.price / 2) * count);
        }
        //������Ʒ
        gameData.bagData.items[index] = null;
        //ͬ��UI��Ʒ����
        if (UIManager.Instance.TryGetWindow(out UI_BagWindow bagWindow))
        {
            bagWindow.UpdateSlot(index, null);
        }
    }
}

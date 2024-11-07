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
        //黑幕遮挡
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
        //判断是否有足够的金币，不足则进行金币闪烁的提醒
        if (gameData.coinCount<itemConfig.price)
        {
            mainWindow.CoinFlash();
            return;
        }
        //判断是否有足够的金币
        //增加物品
        //武器必须占据一个空格子
        if (itemConfig is WeaponConfig)
        {
            if (gameData.bagData.items[index] != null) return;
            //复制一份相同的数据保存到背包中国
            gameData.bagData.items[index] = itemConfig.GetDefaultData().Copy();
        }
        //消耗品要优先考虑堆叠
        else if (itemConfig is ConsunableConfig)
        {
            //尝试找到已经有的同名物品进行叠加
            if (gameData.bagData.TryGetItem(itemConfig.name, out int existIndex))
            {
                ConsumableData consumableData = (ConsumableData)gameData.bagData.items[existIndex];
                consumableData.count += ((ConsumableData)itemConfig.GetDefaultData()).count;
                index = existIndex;
            }
            //要放一个空位
            else
            {
                if (gameData.bagData.items[index] != null) return;
                gameData.bagData.items[index] = itemConfig.GetDefaultData().Copy();
            }
        }
        //同步金币
        SetCoin(gameData.coinCount - itemConfig.price);
        //同步UI物品格子
        if (UIManager.Instance.TryGetWindow(out UI_BagWindow bagWindow))
        {
            bagWindow.UpdateSlot(index, gameData.bagData.items[index]);
        }

    }

    public void SellItem(int index)
    {
        ItemDataBase itemData = gameData.bagData.items[index];
        ItemConfigBase itemConfig = ResManager.Instance.GetItemConfig(itemData.id);
        //增加金币
        if(itemData is WeaponData)
        {
            SetCoin(gameData.coinCount + itemConfig.price / 2);
        }
        else if(itemData is ConsumableData)
        {
            int count = ((ConsumableData)itemData).count;
            SetCoin((gameData.coinCount + itemConfig.price / 2) * count);
        }
        //消除物品
        gameData.bagData.items[index] = null;
        //同步UI物品格子
        if (UIManager.Instance.TryGetWindow(out UI_BagWindow bagWindow))
        {
            bagWindow.UpdateSlot(index, null);
        }
    }
}

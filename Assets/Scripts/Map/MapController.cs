using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public static MapController current;
    public static bool HaveMap=>current!=null&&current.gameObject!=null;
    [SerializeField] private ParallaxScrollingController ParallaxScrollingController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform decorationRoot;
    [SerializeField] private MapConfig mapConfig;

    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private int mapSeed;//��ͼ����
    [SerializeField] private PlayerController playerController; //���Ӧ���и��ϼ��Ĺ�������ʼ������
    public bool hasPlayer => playerController != null;

    //key��ͼ��������value����ͼ�����
    private Dictionary<int,MapChunk> mapChunkDic = new Dictionary<int,MapChunk>();
    private float cellSize;//���ӳߴ�
    public EnemyManager EnemyManager { get=>enemyManager; }

    private float halfCellSize;//��ͼ����


    public float lastCameraPosx { get; private set; } = float.MaxValue;
    public float playerControllerPoxX => playerController.transform.position.x;

    private void Awake()
    {
        //Init(mapSeed,playerController);
        //CreateMapChunk(0);
        current = this;
    }

    public void Init(int mapSeed,PlayerController playerController)
    {
        current = this;
        this.mapSeed = mapSeed;
        this.playerController = playerController;
        groundTileMap.ClearAllTiles();

        Grid grid = GetComponentInChildren<Grid>();
        cellSize = grid.cellSize.x;
        halfCellSize = cellSize / 2;
        cameraController.Init(playerController.transform);
        ParallaxScrollingController.Init(cameraController.screenWidth);
    }

    public Vector3 GetPlayerDefaultPosition()
    {
        return mapConfig.playerDefaultPosition;
    }

    private void LateUpdate()
    {
        if (!hasPlayer) return;
        UpdateMapChunk();
        CheckMapChunkDestroy();
    }

    public void UpdateMapChunk()
    {
        if (cameraController.transform.position.x != lastCameraPosx)
        {
            float oldTargetPosX = lastCameraPosx;
            float newTargetPosX = cameraController.transform.position.x;
            lastCameraPosx = cameraController.transform.position.x;
            //�����Ӳ�
            ParallaxScrollingController.UpdateLyayrs(newTargetPosX);
            //���µ�ͼ
            CheckMapChunkCreation(oldTargetPosX, newTargetPosX);
            lastCameraPosx = newTargetPosX;
        }
    }

    //����ͼ�������
    private void CheckMapChunkCreation(float oldTargetPosX,float newTargetPosX)
    {
        int oldChunkIndex = (int)(oldTargetPosX / (mapConfig.chunkSize * cellSize));
        int newChunkIndex = (int)(newTargetPosX / (mapConfig.chunkSize * cellSize));
        if (oldChunkIndex != newChunkIndex)//��ҵ���һ���µĵ�ͼ��
        {
            //����п����Ǵ������ģ�Ҳ���ǿ�Խ�˺ܶ����ͼ��
            //�رվɵĵ�ͼ��
            DisableMapChunk(oldChunkIndex);
            DisableMapChunk(oldChunkIndex - 1);
            DisableMapChunk(oldChunkIndex + 1);
            //�����µĵ�ͼ��
            EnableMapChunk(newChunkIndex);
            EnableMapChunk(newChunkIndex - 1);
            EnableMapChunk(newChunkIndex + 1);
        }
    }

    //�رյ�ͼ��
    private void DisableMapChunk(int chunkCoord)
    {
        if (chunkCoord < 0) return;
        if(mapChunkDic.TryGetValue(chunkCoord,out MapChunk mapChunk))
        {
            mapChunk.SetActive(false);
        }
    }
    //������ͼ��
    private void EnableMapChunk(int chunkCoord)
    {
        if (chunkCoord < 0) return;
        if(mapChunkDic.TryGetValue(chunkCoord,out MapChunk mapChunk))//������֪ͨ�䲻Ҫ����
        {
            mapChunk.SetActive(true);
        }
        else//�������򴴽�
        {
            mapChunk = new MapChunk();
            mapChunkDic.Add(chunkCoord, mapChunk);
            mapChunk.Init(mapConfig, groundTileMap,transform, decorationRoot, chunkCoord, mapSeed, cellSize, halfCellSize);
        }
       
    }
    private List<int> destoryMapChunkList = new List<int>();
    private void CheckMapChunkDestroy()
    {
        foreach(KeyValuePair<int,MapChunk> item in mapChunkDic)
        {
            if (item.Value.CheckDestroy())
            {
                destoryMapChunkList.Add(item.Key);
            }
        }

        foreach(var chunkCoord in destoryMapChunkList)
        {
            mapChunkDic.Remove(chunkCoord);
        }
        destoryMapChunkList.Clear();
    }

    public int GetCellCoord(float pos)
    {
        return (int)(pos / cellSize);
    }

    public bool CheckCoordIsSedondLayer(int coord)
    {
        return groundTileMap.GetTile(new Vector3Int(coord, 1, 0)) != null;
    }

    public bool IsEmptyCell(int coord)
    {
        return groundTileMap.GetTile(new Vector3Int(coord, 0, 0)) == null;
    }

    public float GetCellWorldPostion(int coord)
    {
        return cellSize * coord - halfCellSize; 
    }

    public float GetLayerWorldPosition(bool secondLayer)
    {
        return cellSize * (secondLayer ? 2 : 1) ;
    }

    public int GetChunkCoord(float pos)
    {
        return (int)(pos / cellSize/mapConfig.chunkSize);
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
        current = null;
    }
}

    
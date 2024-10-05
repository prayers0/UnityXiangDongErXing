using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Dictionary<int,HashSet<EnemyController>> enemyDic=new Dictionary<int,HashSet<EnemyController>>();
    public void AddEnemy(GameObject prefab,int mapChunkCoord, Vector3 pos)
    {
        EnemyController enemyController = GameObject.Instantiate(prefab,transform).GetComponent<EnemyController>();
        
        enemyController.transform.position = pos;
        enemyController.Init();
        if(!enemyDic.TryGetValue(mapChunkCoord,out HashSet<EnemyController> enemys))
        {
            enemys=new HashSet<EnemyController>();
            enemyDic.Add(mapChunkCoord, enemys);
        }
        enemys.Add(enemyController);
    }

    public void UpdateEnemyChunk(EnemyController enemy,int oldCoord,int newCoord)
    {
        if (enemyDic.TryGetValue(oldCoord, out HashSet<EnemyController> oldChunkEnemys))
        {
            oldChunkEnemys.Remove(enemy);
        }
        if (!enemyDic.TryGetValue(newCoord, out HashSet<EnemyController> newChunkEnemys))
        {
            newChunkEnemys = new HashSet<EnemyController>();
            enemyDic.Add(newCoord, newChunkEnemys);
        }
        newChunkEnemys.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy,int mapChunkCoord)
    {
        if (enemyDic.TryGetValue(mapChunkCoord, out HashSet<EnemyController> enemys))
        {
            enemys.Remove(enemy);
        }
        Destroy(enemy.gameObject);
    }

    public void RemoveMapchunkEnemys(int mapChunkCooord)
    {
        if(enemyDic.Remove(mapChunkCooord,out HashSet<EnemyController> enemys))
        {
            foreach(EnemyController enemy in enemys)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lean.Pool;

[Serializable]
public struct AllEnemy
{
    public string id;
    public int amount;
    public float timeSpawn;
    public int maxSpawn;
}

public class AreaEnemy : MonoBehaviour
{
    public float range;
    public List<AllEnemy> allEnemies;
    public List<List<EnemyBase>> listEnemies = new List<List<EnemyBase>>();

    public List<float> timers = new List<float>();

    private void Start()
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            List<EnemyBase> tmp = new List<EnemyBase>();
            listEnemies.Add(tmp);
            timers.Add(0);
        }
    }

    private void Update()
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (listEnemies[i].Count < allEnemies[i].amount)
            {
                timers[i] += Time.deltaTime;
                if (timers[i] > allEnemies[i].timeSpawn)
                {
                    timers[i] = 0;
                    EnemyBase enemyBase = LeanPool.Spawn(AssetsSO.Instance.enemyAssetsSO.enemyAssets[allEnemies[i].id], transform);
                    enemyBase.listEnemyId = i;
                    listEnemies[i].Add(enemyBase);
                    enemyBase.InitEnemy(this);
                }
            }
            else
            {
                timers[i] = 0;
            }
        }
    }

    public void InitArea()
    {
        for(int i=0;i< allEnemies.Count; i++)
        {
            for(int j=0;j< allEnemies[i].amount; j++)
            {
                EnemyBase enemyBase = LeanPool.Spawn(AssetsSO.Instance.enemyAssetsSO.enemyAssets[allEnemies[i].id],transform);
                enemyBase.listEnemyId = i;
                listEnemies[i].Add(enemyBase);
                enemyBase.InitEnemy(this);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}



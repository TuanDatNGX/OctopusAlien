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
}

public class AreaEnemy : MonoBehaviour
{
    public float range;
    public List<AllEnemy> allEnemies;
    Vector2 randomPosition2D;

    private void Start()
    {
        InitArea();
    }

    public void InitArea()
    {
        for(int i=0;i< allEnemies.Count; i++)
        {
            for(int j=0;j< allEnemies[i].amount; j++)
            {
                EnemyBase enemyBase = LeanPool.Spawn(AssetsSO.Instance.enemyAssetsSO.enemyAssets[allEnemies[i].id],transform);
                randomPosition2D = UnityEngine.Random.insideUnitCircle * range;
                enemyBase.transform.position = new Vector3(randomPosition2D.x, 0, randomPosition2D.y);
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



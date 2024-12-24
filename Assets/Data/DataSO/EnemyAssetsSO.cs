using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetsEnemy", menuName = "Assets/AssetsEnemy")]
public class EnemyAssetsSO : SerializedScriptableObject
{
    public Dictionary<string, EnemyBase> enemyAssets = new Dictionary<string, EnemyBase>();
}

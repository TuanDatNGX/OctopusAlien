using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetsLevelUp", menuName = "Assets/AssetsLevelUp")]
public class LevelDataAssetsSO : SerializedScriptableObject
{
    public Dictionary<string, LevelData> enemyAssets = new Dictionary<string, LevelData>();
}

[Serializable]
public class LevelData
{
    public float exp;
    public float evol;
    public float size;
    public float Atk;
    public float Hp;
    public float Heal;
    public float speed;
    public float CatchingRadius;
    public float tentacles;
}

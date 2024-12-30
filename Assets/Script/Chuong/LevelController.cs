using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public int currentLevel
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentLevel", 1);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentLevel", value);
        }
    }
    public bool isTest;
    public int levelTest;

    void Awake()
    {
        Instance = this;
        if(!isTest)
        LoadLevel(currentLevel);
        else LoadLevel(levelTest);
    }

    void LoadLevel(int _level)
    {
        Level level = Instantiate(Resources.Load<GameObject>("Levels/Level" + _level.ToString()).gameObject, transform).GetComponent<Level>();
    }
}

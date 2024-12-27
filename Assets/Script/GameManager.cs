using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : KBTemplate.Patterns.Singleton.Singleton<GameManager>
{
    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    public Camera mainCamera;
    public HpBarController hpBar;
    public GameObject bloodAlien;
}

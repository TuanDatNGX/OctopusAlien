using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : KBTemplate.Patterns.Singleton.Singleton<GameManager>
{
    public Camera mainCamera;
    public HpBarController hpBar;
}

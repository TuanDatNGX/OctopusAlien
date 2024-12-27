using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsBoard : MonoBehaviour
{
    public static StatsBoard Instance;

    private void Awake()
    {
        Instance = this;
    }

    public CharacterStatsBase stats;

    public TextMeshProUGUI atk;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI heal;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI radius;
    public TextMeshProUGUI tentacle;

    public void UpdateData(CharacterStatsBase stat)
    {
        atk.text = "Atk: " + stat.attack.ToString();
        hp.text = "Hp: " + stat.hp.ToString();
        heal.text = "Heal: " + stat.heal.ToString();
        speed.text = "Move speed: " + stat.moveSpeed.ToString();
        radius.text = "Radius: " + stat.rangeAttack.ToString();
        tentacle.text = "Tentacles: " + stat.tentacle.ToString();
    }
}

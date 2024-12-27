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

    public CharacterStat stats;

    public TextMeshProUGUI atk;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI heal;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI radius;
    public TextMeshProUGUI tentacle;
    private void OnEnable()
    {
        if(stats != null)
        UpdateData(stats);
    }

    public void UpdateData(CharacterStat stat)
    {
        atk.text = "Atk: " + stat.ATK.ToString();
        hp.text = "Hp: " + stat.HP.ToString();
        heal.text = "Heal: " + stat.Heal.ToString();
        speed.text = "Move speed: " + stat.MoveSpeed.ToString();
        radius.text = "Radius: " + stat.Radius.ToString();
        tentacle.text = "Tentacles: " + stat.Tentacle.ToString();
    }
}

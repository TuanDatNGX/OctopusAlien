using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    [SerializeField] ProgressBarPro progressBarPro;
    [SerializeField] TextMeshProUGUI txtProcess;
    [SerializeField] TextMeshProUGUI lvlTxt;
    [SerializeField] float speedReload;

    [SerializeField] ProgressBarPro progressBarProExp;
    [SerializeField] TextMeshProUGUI txtProcessExp;

    public void SetValue(float currentValue, float maxValue)
    {
        progressBarPro.SetValue(currentValue, maxValue);
        txtProcess.text = ((int)currentValue).ToString() + "/" + maxValue.ToString();
    }

    public void SetValueExp(float currentValue, float maxValue)
    {
        progressBarProExp.SetValue(currentValue, maxValue);
        txtProcessExp.text = ((int)currentValue).ToString() + "/" + maxValue.ToString();
    }

    public void SetLvl(int lvl)
    {
        lvlTxt.text = "Lv." + lvl.ToString();
    }
}

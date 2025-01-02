using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    [SerializeField] ProgressBarPro progressBarPro;
    [SerializeField] TextMeshProUGUI txtProcess;
    [SerializeField] float speedReload;

    public void SetValue(float currentValue, float maxValue)
    {
        progressBarPro.SetValue(currentValue, maxValue);
        txtProcess.text = ((int)currentValue).ToString() + "/" + maxValue.ToString();
    }
}

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

    public void SetValue(float _value)
    {
        progressBarPro.SetValue(_value,1);
        txtProcess.text = (int)(_value * 100) + ConstantKeyword.keyPhanTram;
    }
}

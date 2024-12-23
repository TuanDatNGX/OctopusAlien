using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UiController : MonoBehaviour
{
    public static UiController Instance;
    public Transform expTransform;
    public TextMeshProUGUI[] expObj;
    int expId = 0;

    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI ExpTxt;

    public void ShowExp(Vector3 pos)
    {
        GameObject exp = expObj[expId].gameObject;
        expId++;
        if (expId >= expObj.Length) expId = 0;
        exp.transform.localPosition = new Vector3(0, -110, 0);
        exp.gameObject.SetActive(true);
        exp.transform.DOLocalMoveY(exp.transform.localPosition.y + 50f, 0.55f).OnComplete(() => {
            exp.gameObject.SetActive(false);
        });
    }
}

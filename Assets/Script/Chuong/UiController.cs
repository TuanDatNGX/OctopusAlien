using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public static UiController Instance;
    public Transform expTransform;
    public TextMeshProUGUI[] expObj;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI killCoutTxt;
    public TextMeshProUGUI timeCoutTxt;
    public GameObject dragToMove;
    public Slider ExpSlider;
    int expId = 0;
    Tween expTween;

    private void Awake()
    {
        Instance = this;
    }


    public TextMeshProUGUI ExpTxt;

    public void ShowExp(Vector3 pos, float expNum)
    {
        GameObject exp = expObj[expId].gameObject;
        expObj[expId].text = "+" + expNum.ToString() + " Exp";
        expId++;
        if (expId >= expObj.Length) expId = 0;
        exp.transform.localPosition = new Vector3(0, -110, 0);
        exp.gameObject.SetActive(true);
        exp.transform.DOLocalMoveY(exp.transform.localPosition.y + 50f, 0.55f).OnComplete(() => {
            exp.gameObject.SetActive(false);
        });
    }

    public void UpdateExp(float current, float max, int level, int killCount)
    {
        expTween.Kill();
        expTween = ExpSlider.DOValue(current / max, 0.25f);
        levelTxt.text = "Level " + level.ToString();
        killCoutTxt.text = "Kill " + killCount.ToString();
    }

    public void ButtonReset()
    {
        SceneManager.LoadScene("Gameplay");
    }
}

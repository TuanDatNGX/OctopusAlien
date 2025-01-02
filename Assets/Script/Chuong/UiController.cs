using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class UiController : MonoBehaviour
{
    public static UiController Instance;
    public GameObject winPopup;
    public GameObject losePopup;
    public Transform expTransform;
    public Transform arrowTransform;
    public TextMeshProUGUI[] expObj;
    public TextMeshProUGUI[] hpObj;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI killCoutTxt;
    public TextMeshProUGUI timeCoutTxt;
    public TextMeshProUGUI stageTxt;
    public TextMeshProUGUI enemyLeftTxt;
    public TextMeshProUGUI expTxt;
    public TextMeshProUGUI questTxt;
    public GameObject dragToMove;
    public ArrowObj arrowObj;
    public Slider ExpSlider;
    int expId = 0;
    Tween expTween;

    private void Awake()
    {
        Instance = this;
        stageTxt.text = "Stage " + LevelController.Instance.currentLevel.ToString();
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

    public void ShowHp(Vector3 pos, float expNum)
    {
        GameObject hp = hpObj[expId].gameObject;
        hpObj[expId].text = "+" + expNum.ToString() + " Hp";
        expId++;
        if (expId >= expObj.Length) expId = 0;
        hp.transform.localPosition = new Vector3(0, -110, 0);
        hp.gameObject.SetActive(true);
        hp.transform.DOLocalMoveY(hp.transform.localPosition.y + 50f, 0.55f).OnComplete(() => {
            hp.gameObject.SetActive(false);
        });
    }

    public void UpdateExp(float current, float max, int level, int killCount)
    {
        expTween.Kill();
        expTween = ExpSlider.DOValue(current / max, 0.25f);
        levelTxt.text = "Lv." + level.ToString();
        killCoutTxt.text = "Kill " + killCount.ToString();
        expTxt.text = current.ToString() + "/" + max.ToString();
    }

    //private void Update()
    //{
    //    int cout = 0;
    //    if (LevelController.Instance.Level.questType == QuestType.Survise)
    //    {
    //        if (LevelController.Instance.Level != null)
    //            foreach (Transform t in LevelController.Instance.Level.BotTransform)
    //            {
    //                if (t.gameObject.activeSelf) cout++;
    //            }
    //        UpdateEnemiesLeft(cout);

    //        if (cout <= 0)
    //        {
    //            if (LevelController.Instance.Level != null)
    //            {
    //                LevelController.Instance.Level.StopCount();
    //                winPopup.SetActive(true);
    //            }
    //        }
    //    }
    //}

    public void UpdateEnemiesLeft(int count)
    {
        enemyLeftTxt.text = "Alive: " + count.ToString();
    }

    public void ButtonReset()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void SetupArrow(Transform botTransform)
    {
        foreach (Transform child in botTransform)
        {
            ArrowObj arrow = Instantiate(arrowObj, arrowTransform);
            arrow.target = child.GetComponent<AICharacterController>();
            arrow.gameObject.SetActive(true);
        }
    }

    public void UpdateQuestProcess(Level level)
    {
        questTxt.text = level.questName + " " + level.currentProcess.ToString() + "/" + level.questTargetValue.ToString();
    }

    public void StartStage()
    {
        UiController.Instance.dragToMove.gameObject.SetActive(false);
        LevelController.Instance.Level.StartLevel();
    }
}

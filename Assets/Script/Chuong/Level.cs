using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Survise,
    GetLevel,
    KillAlien
}

public class Level : MonoBehaviour
{
    public QuestType questType;
    public int questTargetValue;
    public string questName;
    public int currentProcess { get; set; } = 0;
    public Transform BotTransform;
    public float levelTime;
    float curTime;
    public Coroutine timeCou;

    private void Start()
    {
        UiController.Instance.SetupArrow(BotTransform);
        BotTransform.gameObject.SetActive(false);
        UiController.Instance.timeCoutTxt.text = levelTime.ToString();
        if (questType == QuestType.GetLevel)
        {
            currentProcess = 1;
        }
        UiController.Instance.UpdateQuestProcess(this);
    }

    public void Process(int value)
    {
        currentProcess += value;
        UiController.Instance.UpdateQuestProcess(this);
        if(currentProcess >= questTargetValue)
        {
            UiController.Instance.winPopup.SetActive(true);
        }
    }

    public void StopCount()
    {
        StopCoroutine(TimeCount());
    }

    public void StartLevel()
    {
        BotTransform.gameObject.SetActive(true);
        timeCou = StartCoroutine(TimeCount());
    }

    IEnumerator TimeCount()
    {
        curTime = levelTime;
        while (curTime >= 0)
        {
            UiController.Instance.timeCoutTxt.text = curTime.ToString();
            yield return new WaitForSeconds(1);
            curTime -= 1 / Time.timeScale;
            if (curTime < 0)
            {
                //int cout = 0;
                //foreach (Transform t in BotTransform)
                //{
                //    if (transform.gameObject.activeSelf)
                //    {
                //        cout++;
                //    }
                //}
                //if(cout <= 0)
                //{
                //    UiController.Instance.winPopup.SetActive(true);
                //}
                //else
                //{
                //    UiController.Instance.losePopup.SetActive(true);
                //}
                UiController.Instance.losePopup.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform BotTransform;
    public float levelTime;
    public float topCondition;
    float curTime;
    public Coroutine timeCou;

    private void Start()
    {
        UiController.Instance.SetupArrow(BotTransform);
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
                int cout = 0;
                foreach (Transform t in BotTransform)
                {
                    if (transform.gameObject.activeSelf)
                    {
                        cout++;
                    }
                }
                if(cout <= 2)
                {
                    UiController.Instance.winPopup.SetActive(true);
                }
                else
                {
                    UiController.Instance.losePopup.SetActive(true);
                }
            }
        }
    }
}

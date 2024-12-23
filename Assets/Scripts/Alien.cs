using DG.Tweening;
using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AlienState
{
    Idle,
    Catched,
    Collect,
}

public class Alien : MonoBehaviour
{
    public AlienState currentState = AlienState.Idle;
    public OctopusTail target;
    public SphereCollider collider;
    public GameObject hpObj;
    public Image hpFill;
    public float currentHp = 100;

    private void Update()
    {
        switch (currentState)
        {
            case AlienState.Idle:
                currentHp += 100 * Time.deltaTime;
                hpFill.fillAmount = currentHp / 100f;
                if (currentHp >= 100) currentHp = 100;
                hpObj.SetActive(currentHp < 100);
                break;
            case AlienState.Catched:
                currentHp -= 100 * Time.deltaTime;
                hpFill.fillAmount = currentHp / 100f;
                hpObj.SetActive(currentHp < 100);
                if (currentHp <= 0)
                {
                    currentHp = 0;
                    ChangeState(AlienState.Collect);
                }
                break;
            case AlienState.Collect:
                break;
        }
    }

    public void ChangeState(AlienState state)
    {
        if (currentState == state) return;
        currentState = state;
        switch (state)
        {
            case AlienState.Idle:
                target.ChangeState(TailState.Idle);
                target = null;
                break;
            case AlienState.Catched:
                break;
            case AlienState.Collect:
                target.player.listAlienInRange.Remove(this);
                collider.enabled = false;
                target.ChangeState(TailState.Collect);
                transform.DOMove(target.transform.position, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    target.ChangeState(TailState.Idle);
                    gameObject.SetActive(false);
                });
                break;
        }
    }
}

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
    public AlienSpawnCircle circle;
    public Animator animator;
    public Transform hit;
    public AlienState currentState = AlienState.Idle;
    public OctopusTail target;
    public SphereCollider collider;
    public GameObject growingRoot;
    public GameObject hpObj;
    public Image hpFill;
    public float currentHp = 100;
    public float Speed;
    Tween moving;
    Vector3 des;

    private void Start()
    {
        Move();
        animator.SetFloat("Speed", 1);
    }

    private void OnDisable()
    {
        moving.Kill();
    }

    public void Move()
    {
        des = new Vector3(Random.Range(-circle.radius, circle.radius), 0, Random.Range(-circle.radius, circle.radius));
        animator.transform.LookAt(circle.transform.position + des);
        moving = transform.DOLocalMove(des, Vector3.Distance(transform.localPosition, des) / Speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            if(gameObject.activeSelf)
            Move();
        });
    }

    private void Update()
    {
        if(moving != null && Vector3.Distance(transform.localPosition, des) > 0.1f)
        {

        }

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
                animator.SetFloat("Speed", 0);
                moving.Kill();
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
                growingRoot.SetActive(false);
                target.ChangeState(TailState.Idle);
                target = null;
                break;
            case AlienState.Catched:
                growingRoot.SetActive(true);
                break;
            case AlienState.Collect:
                target.player.listAlienInRange.Remove(this);
                collider.enabled = false;
                target.ChangeState(TailState.Collect);
                transform.DOMove(target.transform.position, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    target.ChangeState(TailState.Idle);
                    UiController.Instance.ShowExp(transform.position);
                    gameObject.SetActive(false);
                });
                break;
        }
    }
}

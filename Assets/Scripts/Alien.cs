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
    public float maxHp = 100;
    public float Speed;
    public float heal = 100;
    public int rewardExp;
    Tween moving;
    Vector3 des;

    private void Start()
    {
        Move();
        currentHp = maxHp;
        animator.SetFloat("Speed", 1);
    }

    private void OnDisable()
    {
        moving.Kill();
    }

    public void Move()
    {
        des = new Vector3(Random.Range(-circle.radius, circle.radius), 0, Random.Range(-circle.radius, circle.radius));
        transform.LookAt(circle.transform.position + des);
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
                currentHp += heal * Time.deltaTime;
                hpFill.fillAmount = currentHp / maxHp;
                if (currentHp >= maxHp) currentHp = maxHp;
                hpObj.SetActive(currentHp < maxHp);
                break;
            case AlienState.Catched:
                currentHp -= target.player.GetComponent<CharacterStat>().ATK * Time.deltaTime;
                hpFill.fillAmount = currentHp / maxHp;
                hpObj.SetActive(currentHp < maxHp);
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
                //animator.Play("Floating");
                target.player.listAlienInRange.Remove(this);
                collider.enabled = false;
                target.ChangeState(TailState.Collect);

                transform.DOMove(transform.position + new Vector3(0, 0.5f, 0), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOMove(target.player.transform.position + new Vector3(0, -0.25f, 0), 0.35f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        target.ChangeState(TailState.Idle);
                        target.player.GetExp(rewardExp);
                        UiController.Instance.ShowExp(transform.position, rewardExp);
                        EffectController.Instance.SpawnBloodFx(transform.position);
                        EffectController.Instance.SpawnBloodFx2(new Vector3(transform.position.x, 0, transform.position.z));
                        gameObject.SetActive(false);
                    });
                });
                break;
        }
    }
}

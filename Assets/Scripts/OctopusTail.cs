using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TailState
{
    Idle,
    Catch,
    Collect,
}

public class OctopusTail : MonoBehaviour
{
    public PlayerController player;
    public TailAnimator2 tailAnimator;
    public TailState currentState = TailState.Idle;
    public EnemyBase target;
    public ParticleSystem[] effectTail;
    Vector3 defaultRotation;
    float currentBlend = 0;
    float speedCollect = 5;
    bool canCatch = false;

    private void Awake()
    {
        defaultRotation = transform.localEulerAngles;
    }

    private void OnEnable()
    {
        tailAnimator.LengthMultiplier = 0;

        DOTween.To(() => tailAnimator.LengthMultiplier, x => tailAnimator.LengthMultiplier = x, 1f, 0.5f).OnComplete(() =>
        {
            foreach (var effect in effectTail)
            {
                effect.Play();
            }
            canCatch = true;
        });
    }

    private void Update()
    {
        //if (currentState == TailState.Catch)
        //{
        //    transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        //}
        switch (currentState)
        {
            case TailState.Idle:
                break;
            case TailState.Catch:
                CheckTargetOutRange();
                if (target != null)
                {
                    if (target.TakeDamage(this))
                    {
                        if (!tailAnimator.IKTarget)
                        {
                            transform.DORewind(true);
                            tailAnimator.IKTarget = target.hit;
                            tailAnimator.UseIK = true;
                            tailAnimator.TailAnimatorAmount = 0.85f;
                            currentBlend = 0;
                            tailAnimator.IKBlend = 0;
                            tailAnimator.IKContinousSolve = true;
                            tailAnimator.Slithery = 1f;
                        }
                        currentBlend += 4 * Time.deltaTime;
                        if (currentBlend >= 1) currentBlend = 1;
                        tailAnimator.IKBlend = currentBlend;
                    }
                }
                break;
            case TailState.Collect:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == TailState.Catch)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
    }

    public void ChangeState(TailState state)
    {
        if (currentState == state) return;
        currentState = state;
        switch (state)
        {
            case TailState.Idle:
                target = null;
                tailAnimator.UseIK = false;
                tailAnimator.IKTarget = null;
                tailAnimator.TailAnimatorAmount = 1f;
                tailAnimator.Slithery = 0.67f;
                transform.DOLocalRotate(defaultRotation, 2);
                break;
            case TailState.Catch:
                //tailAnimator.UseIK = true;
                //tailAnimator.TailAnimatorAmount = 0.85f;
                //currentBlend = 0;
                //tailAnimator.IKBlend = 0;
                //tailAnimator.IKContinousSolve = true;
                //tailAnimator.Slithery = 1f;
                break;
            case TailState.Collect:
                tailAnimator.TailAnimatorAmount = 0.15f;
                break;
        }
    }

    public void CatchAlien(EnemyBase alien)
    {
        //if (!canCatch) return;
        target = alien;
        //alien.target = this;
        //alien.ChangeState(AlienState.Catched);
        ChangeState(TailState.Catch);
    }

    void CheckTargetOutRange()
    {
        if (!player.listAlienInRange.Contains(target))
        {
            target = null;
            ChangeState(TailState.Idle);
        }
    }

    public void CollectTarget()
    {
        player.listAlienInRange.Remove(target);
        ChangeState(TailState.Collect);
        StartCoroutine(CollectingTarget());
    }

    IEnumerator CollectingTarget()
    {
        player.ActionEat();

        while (true)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, player.mouth.position, speedCollect * Time.deltaTime);
            if(Vector3.Distance(target.transform.position, player.mouth.position) < .1f)
            {
                target.AffterDie();
                player.GetExp(target.statsBase.rewardExp);
                EffectController.Instance.SpawnBloodFx(target.transform.position);
                target = null;
                ChangeState(TailState.Idle);
                break;
            }

            yield return null;
        }
    }
}

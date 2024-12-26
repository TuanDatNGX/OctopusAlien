using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lean.Pool;

public enum State
{
    Idle,
    RunAway,
    Attack,
    Die
}
[Serializable]
public struct StatsBase
{
    public string id;
    public float hp;
    public float attack;
    public float delayAttack;
    public float moveSpeed;
    public float levelAI;
    public float rewardExp;
}

[Serializable]
public struct StatsAIEnemy
{
    public float detectionRange;
    public int numDirections;
    public float timeDelayNextTarget;
}

public abstract class EnemyBase : MonoBehaviour
{
    public Animator aniEnemy;
    public StatsBase statsBase;
    public StatsAIEnemy statsAIEnemy;
    public State stateNow;
    public float hpNow;
    public AreaEnemy myArea;
    public Vector3 targetMove;
    public Vector2 randomTarget;
    public float rotateMultiplier;
    public float countTimeDelayNextTarget;
    public List<Transform> listAttacker;
    public Vector3 directionTarget;
    public HpBarController hpBar;
    Vector2 randomPosition2D;

    public void InitEnemy(AreaEnemy _areaEnemy)
    {
        myArea = _areaEnemy;
        randomPosition2D = UnityEngine.Random.insideUnitCircle * myArea.range;
        transform.position = new Vector3(randomPosition2D.x, 0, randomPosition2D.y);
        randomTarget = UnityEngine.Random.insideUnitCircle * myArea.range;
        targetMove = new Vector3(randomTarget.x, 0, randomTarget.y);
        ChangeState(State.Idle);
    }

    public abstract void Idle();
    public abstract void RunAway();
    public abstract void Attack();

    public virtual void Die()
    {
        aniEnemy.SetFloat("Speed", 0f);
        StartCoroutine(CountDownRevive());
    }

    private void Update()
    {
        UpdateState();
    }

    void UpdateHp(float _value)
    {
        hpNow += _value;
        hpBar.SetValue(hpNow / statsBase.hp);
        hpBar.transform.position = GameManager.Instance.mainCamera.WorldToScreenPoint(transform.position);

        switch (stateNow)
        {
            case State.Idle:
                if (hpNow >= statsBase.hp)
                {
                    LeanPool.Despawn(hpBar);
                }
                break;
            case State.RunAway:

                if (hpNow <= 0)
                {
                    ChangeState(State.Die);
                }
                break;
            case State.Attack:
                break;
        }
    }

    void UpdateState()
    {
        if (listAttacker.Count > 0)
        {
            ChangeState(State.RunAway);
        }
        else
        {
            ChangeState(State.Idle);
        }

        switch (stateNow)
        {
            case State.Idle:
                Idle();
                MoveToDirection();
                if (hpNow < statsBase.hp)
                {
                    //UpdateHp();
                }
                break;
            case State.RunAway:
                RunAway();
                MoveToDirection();
                //UpdateHp();
                break;
            case State.Attack:
                break;
            case State.Die:
                break;
        }
    }

    void MoveToDirection()
    {
        if (stateNow == State.Idle)
        {
            if (Vector3.Distance(transform.position, targetMove) < .1f) return;
        }
        transform.position += directionTarget.normalized * statsBase.moveSpeed * Time.deltaTime;
        float rotateSpeed = statsBase.moveSpeed * rotateMultiplier;

        Quaternion targetRotation = Quaternion.LookRotation(directionTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }


    public void ChangeState(State _stateChange)
    {
        if (_stateChange == stateNow) return;
        stateNow = _stateChange;

        switch (_stateChange)
        {
            case State.Idle:
                aniEnemy.SetFloat("Speed", .5f);
                break;
            case State.RunAway:
                hpBar = LeanPool.Spawn(GameManager.Instance.hpBar, UIManager.Instance.parentHP);
                aniEnemy.SetFloat("Speed", 1f);
                break;
            case State.Attack:
                break;
            case State.Die:
                Die();
                break;
        }
    }

    IEnumerator CountDownRevive()
    {
        yield return new WaitForSeconds(5);
        InitEnemy(myArea);
    }
}

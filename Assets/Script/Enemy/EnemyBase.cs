using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

public abstract class EnemyBase : MonoBehaviour
{
    public StatsBase statsBase;
    public State stateNow;
    public AreaEnemy myArea;
    public Vector3 targetMove;
    public Vector2 randomTarget;
    public float rotateMultiplier;
    public float timeDelayNextTarget;
    public float countTimeDelayNextTarget;

    public void InitEnemy(AreaEnemy _areaEnemy)
    {
        myArea = _areaEnemy;
        randomTarget = UnityEngine.Random.insideUnitCircle * myArea.range;
        targetMove = new Vector3(randomTarget.x, 0, randomTarget.y);
        stateNow = State.Idle;
    }
    public abstract void Move();
    public abstract void RunAway();
    public abstract void Attack();

    private void Update()
    {
        switch (stateNow)
        {
            case State.Idle:
                Move();
                break;
            case State.RunAway:
                RunAway();
                break;
            case State.Attack:
                break;
            case State.Die:
                break;
        }
    }

    public void ChangeState(State _stateChange)
    {
        if (_stateChange == stateNow) return;
        stateNow = _stateChange;
    }
}

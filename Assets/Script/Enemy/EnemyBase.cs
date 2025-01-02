using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lean.Pool;

public enum State
{
    None,
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
    public float timeDelayRunAway;
    public float timeDelayCatch;
}

public abstract class EnemyBase : TargetBase
{
    public Animator aniEnemy;
    public Collider colliderEnemy;
    public StatsBase statsBase;
    public StatsAIEnemy statsAIEnemy;
    public float rotateMultiplier;
    public State stateNow;
    public AreaEnemy myArea;
    public Vector3 targetMove;
    public Vector2 randomTarget;
    public float countTimeDelayNextTarget;
    public Vector3 directionTarget;
    public bool canRunAway;
    public int listEnemyId;
    bool canCatch = true;
    public int HpHeal;
    Vector2 randomPosition2D;
    GameObject blood;
    Coroutine coroutineDelayCatch, coroutineDelayRunAway;


    public void InitEnemy(AreaEnemy _areaEnemy)
    {
        myArea = _areaEnemy;
        colliderEnemy.enabled = true;
        aniEnemy.gameObject.SetActive(true);
        randomPosition2D = UnityEngine.Random.insideUnitCircle * myArea.range;
        transform.position = new Vector3(myArea.transform.position.x + randomPosition2D.x, 0, myArea.transform.position.z + randomPosition2D.y);
        hpNow = statsBase.hp;
        listAttacker.Clear();
        ChangeState(State.Idle);
    }

    public abstract void Idle();
    public abstract void RunAway();
    public abstract void Attack();

    public override void Die()
    {
        
        aniEnemy.Play("Floating");
        colliderEnemy.enabled = false;
        myArea.listEnemies[listEnemyId].Remove(this);
        LeanPool.Despawn(hpBar);
        hpBar = null;
        aniEnemy.SetFloat("Speed", 0f);
    }

    public override bool TakeDamage(OctopusTail _octopusTail)
    {
        if (!listAttacker.Contains(_octopusTail.octopus.gameObject))
        {
            listAttacker.Add(_octopusTail.octopus.gameObject);
        }

        if (canCatch && stateNow != State.Die)
        {
            growingRoot.gameObject.SetActive(true);
            ChangeState(State.RunAway);
            UpdateHp(-_octopusTail.octopus.characterStatsBase.attack, _octopusTail);
            return true;
        }
        return false;
    }

    private void Update()
    {
        UpdateState();
    }

    public override void UpdateHp(float _value, OctopusTail _octopusTail = null)
    {
        if (hpBar == null) return;
        hpNow += (_value * Time.deltaTime);
        hpBar.SetValue(hpNow / statsBase.hp);
        hpBar.transform.position = GameManager.Instance.mainCamera.WorldToScreenPoint(posHpBar.position);

        switch (stateNow)
        {
            case State.Idle:
                if (hpNow >= statsBase.hp)
                {
                    hpNow = statsBase.hp;
                    LeanPool.Despawn(hpBar);
                    hpBar = null;
                }
                break;
            case State.RunAway:
                if (hpNow <= 0)
                {
                    ChangeState(State.Die);
                    _octopusTail.CollectTarget();
                }
                break;
            case State.Attack:
                break;
        }
    }

    void UpdateState()
    {
        if (listAttacker.Count <= 0)
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
                    UpdateHp(statsBase.hp * 0.5f);
                }
                break;
            case State.RunAway:
                if (canRunAway)
                {
                    RunAway();
                }
                else
                {
                    Idle();
                }
                MoveToDirection();
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
            if (Vector3.Distance(transform.position, targetMove) < .1f)
            {
                aniEnemy.SetFloat("Speed", 0f);
                return;
            }
        }
        if (stateNow == State.RunAway && canRunAway)
        {
            transform.position += directionTarget.normalized * statsBase.moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMove, statsBase.moveSpeed * Time.deltaTime);
        }
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
                ChangeTargetMove();
                if (coroutineDelayRunAway != null)
                {
                    StopCoroutine(coroutineDelayRunAway);
                }
                StartCoroutine(CountTimeDelayCatch());
                //aniEnemy.SetFloat("Speed", 1f);
                break;
            case State.RunAway:
                if (coroutineDelayCatch != null)
                {
                    StopCoroutine(coroutineDelayCatch);
                }
                StartCoroutine(CountTimeDelayRunAway());
                if (!hpBar)
                {
                    hpBar = LeanPool.Spawn(GameManager.Instance.hpBar, UIManager.Instance.parentHP);
                }
                break;
            case State.Attack:
                break;
            case State.Die:
                Die();
                break;
        }
    }

    public override void Escaped(CharacterBase _characterBase)
    {
        if (listAttacker.Contains(_characterBase.gameObject))
        {
            listAttacker.Remove(_characterBase.gameObject);
        }
        if (listAttacker.Count <= 0)
        {
            if (growingRoot != null)
            {
                growingRoot.SetActive(false);
            }
        }
    }

    public void ChangeTargetMove()
    {
        randomTarget = UnityEngine.Random.insideUnitCircle * myArea.range;
        targetMove = new Vector3(myArea.transform.position.x + randomTarget.x, 0, myArea.transform.position.z + randomTarget.y);
    }

    IEnumerator CountDownRevive()
    {
        yield return new WaitForSeconds(5);
        InitEnemy(myArea);
    }

    IEnumerator CountTimeDelayRunAway()
    {
        canRunAway = false;
        yield return new WaitForSeconds(statsAIEnemy.timeDelayRunAway);
        canRunAway = true;
        aniEnemy.SetFloat("Speed", 1f);
    }

    IEnumerator CountTimeDelayCatch()
    {
        canCatch = false;
        yield return new WaitForSeconds(statsAIEnemy.timeDelayCatch);
        canCatch = true;
    }
}

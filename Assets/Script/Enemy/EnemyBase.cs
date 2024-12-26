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
    public float timeDelayCatch;
}

public abstract class EnemyBase : MonoBehaviour
{
    public Animator aniEnemy;
    public Collider colliderEnemy;
    public Transform posHpBar;
    public StatsBase statsBase;
    public StatsAIEnemy statsAIEnemy;
    public float rotateMultiplier;
    public State stateNow;
    public float hpNow;
    public AreaEnemy myArea;
    public Vector3 targetMove;
    public Vector2 randomTarget;
    public float countTimeDelayNextTarget;
    public List<GameObject> listAttacker;
    public Vector3 directionTarget;
    public HpBarController hpBar;
    Vector2 randomPosition2D;
    bool canCatch=true;
    GameObject blood;


    public void InitEnemy(AreaEnemy _areaEnemy)
    {
        myArea = _areaEnemy;
        colliderEnemy.enabled = true;
        aniEnemy.gameObject.SetActive(true);
        randomPosition2D = UnityEngine.Random.insideUnitCircle * myArea.range;
        transform.position = new Vector3(myArea.transform.position.x+randomPosition2D.x, 0, myArea.transform.position.z+randomPosition2D.y);
        hpNow = statsBase.hp;
        listAttacker.Clear();
        ChangeState(State.Idle);
    }

    public abstract void Idle();
    public abstract void RunAway();
    public abstract void Attack();

    public virtual void Die()
    {
        colliderEnemy.enabled = false;
        LeanPool.Despawn(hpBar);
        hpBar = null;
        aniEnemy.SetFloat("Speed", 0f);
    }

    public void AffterDie()
    {
        blood = LeanPool.Spawn(GameManager.Instance.bloodAlien);
        blood.SetActive(false);
        blood.transform.position = new Vector3(transform.position.x,0, transform.position.z);
        blood.SetActive(true);
        aniEnemy.gameObject.SetActive(false);
        StartCoroutine(CountDownRevive());
    }

    public bool TakeDamage(OctopusTail _octopusTail)
    {
        if (!listAttacker.Contains(_octopusTail.player.gameObject))
        {
            listAttacker.Add(_octopusTail.player.gameObject);
        }

        if (canCatch && stateNow != State.Die)
        {
            ChangeState(State.RunAway);
            UpdateHp(-_octopusTail.player.GetComponent<CharacterStat>().ATK, _octopusTail);
            return true;
        }
        return false;
    }

    public void Escaped(PlayerController _playerController)
    {
        if (listAttacker.Contains(_playerController.gameObject))
        {
            listAttacker.Remove(_playerController.gameObject);
        }
    }

    private void Update()
    {
        UpdateState();
    }

    void UpdateHp(float _value, OctopusTail _octopusTail=null)
    {
        hpNow += (_value*Time.deltaTime);
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
                    //ham cong kinh nghiem _octopusTail
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
                    UpdateHp(50);
                }
                break;
            case State.RunAway:
                RunAway();
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
                ChangeTargetMove();
                StartCoroutine(CountTimeDelayCatch());
                aniEnemy.SetFloat("Speed", 1f);
                break;
            case State.RunAway:
                if (!hpBar)
                {
                    hpBar = LeanPool.Spawn(GameManager.Instance.hpBar, UIManager.Instance.parentHP);
                }
                aniEnemy.SetFloat("Speed", 1f);
                break;
            case State.Attack:
                break;
            case State.Die:
                Die();
                break;
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

    IEnumerator CountTimeDelayCatch()
    {
        canCatch = false;
        yield return new WaitForSeconds(statsAIEnemy.timeDelayCatch);
        canCatch = true;
    }
}

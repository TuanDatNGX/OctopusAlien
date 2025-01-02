using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterController : CharacterBase
{
    public NavMeshAgent navMeshAgent;
    public float randomMoveRadius;
    public string botName;

    private void Start()
    {
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.angularSpeed = rotateSpeed;
        SetTarget();
    }
    public override void Move()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetTarget();
        }
    }

    public override void Attack()
    {
        //SelectTarget(Vector3.zero,)
    }

    void SetTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomMoveRadius;
        randomDirection += transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, randomMoveRadius, NavMesh.AllAreas))
        {
            SelectTarget(hit.position);
        }
    }


    public void SelectTarget(Vector3 _target, Transform _targetTransform = null)
    {
        if (_targetTransform == null)
        {
            navMeshAgent.SetDestination(_target);
        }
        else
        {
            navMeshAgent.SetDestination(_targetTransform.position);
        }
    }

    public override void LevelUp()
    {
        navMeshAgent.speed = moveSpeed;
    }

    public override void AffterDie(CharacterBase _octopus = null)
    {
        _octopus.GetExp(currentLevel * 5);
        gameObject.SetActive(false);
    }

    public override void Die()
    {
        if (LevelController.Instance.Level.questType == QuestType.Survise)
        {
            LevelController.Instance.Level.Process(1);
        }
        navMeshAgent.enabled = false;
        LeanPool.Despawn(hpBar);
        hpBar = null;
    }
}

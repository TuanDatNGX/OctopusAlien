using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterController : CharacterBase
{
    public NavMeshAgent navMeshAgent;
    public float randomMoveRadius;
    private void Start()
    {
        navMeshAgent.speed = characterStatsBase.moveSpeed;
        SetTarget();
    }
    public override void Move()
    {
        if (stateNow == StateCharacter.Idle)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SetTarget();
            }
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
}

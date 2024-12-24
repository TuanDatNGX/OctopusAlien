using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyBase
{
    public override void Attack()
    {
    }

    public override void Move()
    {
        if (Vector3.Distance(transform.position, targetMove) < .1f)
        {
            if (countTimeDelayNextTarget <= 0)
            {
                countTimeDelayNextTarget = timeDelayNextTarget;
            }
            else
            {
                countTimeDelayNextTarget -= Time.deltaTime;
                if (countTimeDelayNextTarget <= 0)
                {
                    randomTarget = Random.insideUnitCircle * myArea.range;
                    targetMove = new Vector3(randomTarget.x, 0, randomTarget.y);
                }
            }
        }
        else
        {
            Vector3 direction = (targetMove - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetMove, statsBase.moveSpeed * Time.deltaTime);

            float rotateSpeed = statsBase.moveSpeed * rotateMultiplier;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    public override void RunAway()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyBase
{
    public override void AffterDie(CharacterBase _octopus)
    {
        if (isOctopus)
        {

        }
        else
        {
            aniEnemy.gameObject.SetActive(false);
            growingRoot.SetActive(false);
            _octopus.GetExp(statsBase.rewardExp);
            EffectController.Instance.SpawnBloodFx(transform.position);
        }
    }

    public override void Attack()
    {
    }

    public override void Idle()
    {
        if (Vector3.Distance(transform.position, targetMove) < .1f)
        {
            if (countTimeDelayNextTarget <= 0)
            {
                countTimeDelayNextTarget = statsAIEnemy.timeDelayNextTarget;
                aniEnemy.SetFloat("Speed", 0f);
            }
            else
            {
                countTimeDelayNextTarget -= Time.deltaTime;
                if (countTimeDelayNextTarget <= 0)
                {
                    ChangeTargetMove();
                    aniEnemy.SetFloat("Speed", 1f);
                }
            }
        }
        else
        {
            Vector3 direction = (targetMove - transform.position).normalized;
            directionTarget = direction;
        }
    }

    public override void RunAway()
    {
        if (listAttacker.Count == 0) return; // Không có player nào thì không làm gì
        // Tính toán hướng ngược lại tất cả các player
        Vector3 totalOppositeDirection = Vector3.zero;
        foreach (GameObject player in listAttacker)
        {
            Vector3 toPlayer = player.transform.position - transform.position;
            if (toPlayer.magnitude <= statsAIEnemy.detectionRange)
            {
                totalOppositeDirection += -toPlayer.normalized; // Cộng hướng ngược lại
            }
        }

        // Bình thường hóa hướng tổng hợp
        Vector3 primaryDirection = totalOppositeDirection.normalized;

        // Tìm hướng tốt nhất dựa trên khoảng trống và kết hợp với hướng ngược lại
        Vector3 bestDirection = Vector3.zero;
        float bestScore = float.MinValue;

        for (int i = 0; i < statsAIEnemy.numDirections; i++)
        {
            // Tính toán hướng
            float angle = i * 360f / statsAIEnemy.numDirections;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            // Tính điểm cho hướng này
            float score = EvaluateDirection(direction, primaryDirection);

            // Cập nhật hướng tốt nhất nếu điểm cao hơn

            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = direction;
            }
        }

        // Di chuyển enemy theo hướng tốt nhất
        if (bestDirection != Vector3.zero)
        {
            directionTarget = bestDirection;
            //transform.position += bestDirection.normalized * statsBase.moveSpeed * Time.deltaTime;
        }
    }

    private float EvaluateDirection(Vector3 direction, Vector3 primaryDirection)
    {
        float score = 0f;

        // Ưu tiên hướng gần với hướng ngược lại player
        float alignmentWithPrimary = Vector3.Dot(direction.normalized, primaryDirection);
        score += alignmentWithPrimary * 2f; // Tăng điểm cho hướng phù hợp

        // Kiểm tra khoảng cách tới các player
        foreach (GameObject player in listAttacker)
        {
            Vector3 toPlayer = player.transform.position - transform.position;
            float distance = toPlayer.magnitude;

            if (distance <= statsAIEnemy.detectionRange)
            {
                // Tính toán sự gần gũi giữa hướng và player
                float alignmentWithPlayer = Vector3.Dot(direction.normalized, toPlayer.normalized);
                score -= Mathf.Clamp01(alignmentWithPlayer); // Trừ điểm nếu gần với hướng player
            }
        }

        // Kiểm tra vật cản bằng Raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, statsAIEnemy.detectionRange))
        {
            // Trừ điểm nếu gặp vật cản
            score -= 1f / hit.distance;
        }
        return score;
    }
}

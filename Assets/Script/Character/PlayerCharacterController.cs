using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : CharacterBase
{
    public CharacterController characterController;
    public FloatingJoystick joystick;
    Vector3 inputDirection;
    float defaultY;
    private void Start()
    {
        //StatsBoard.Instance.UpdateData(characterStatsBase);
        defaultY = transform.position.y;
    }
    public override void Attack()
    {
    }

    public override void LevelUp()
    {
        //StatsBoard.Instance.UpdateData(characterStatsBase);
    }

    public override void Move()
    {
        inputDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        if (inputDirection.magnitude > 0.1f)
        {
            UiController.Instance.dragToMove.gameObject.SetActive(false);
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            characterController.Move(inputDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move(Vector3.zero * Time.deltaTime);
        }
    }

    public override void AffterDie(CharacterBase _octopus = null)
    {
        gameObject.SetActive(false);
    }

    public override void Die()
    {
        LeanPool.Despawn(hpBar);
        hpBar = null;
    }
}

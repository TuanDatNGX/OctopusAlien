using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : CharacterBase
{
    public CharacterController characterController;
    public FloatingJoystick joystick;
    Vector3 inputDirection;
    public override void Attack()
    {
    }

    public override void Move()
    {
        inputDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        if (inputDirection.magnitude > 0.1f)
        {
            UiController.Instance.dragToMove.gameObject.SetActive(false);
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            characterController.Move(inputDirection.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move(Vector3.zero * Time.deltaTime);
        }
    }
}

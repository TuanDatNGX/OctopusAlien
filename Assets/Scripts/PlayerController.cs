using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public FloatingJoystick joystick;
    public float moveSpeed;
    public float rotateSpeed;
    Vector3 inputDirection;
    public OctopusTail[] tails;
    public List<Alien> listAlienInRange;
    Alien neareastAlien;

    private void Update()
    {
        neareastAlien = null;
        foreach (var alien in listAlienInRange)
        {
            float minDistance = 1000;
            if (alien.currentState == AlienState.Idle)
            {
                float distance = Vector3.Distance(transform.position, alien.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    neareastAlien = alien;
                }
            }
        }
        foreach (var tail in tails)
        {
            if(tail.gameObject.activeSelf && tail.currentState == TailState.Idle)
            {
                if(neareastAlien != null && neareastAlien.currentState == AlienState.Idle)
                {
                    tail.CatchAlien(neareastAlien);
                    break;
                }
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        inputDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        if (inputDirection.magnitude > 0.1f)
        {
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

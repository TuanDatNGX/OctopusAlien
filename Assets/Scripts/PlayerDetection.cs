using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public PlayerController controller;
    private void OnTriggerEnter(Collider other)
    {
        controller.listAlienInRange.Add(other.transform.parent.GetComponent<Alien>());
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent.GetComponent<Alien>().ChangeState(AlienState.Idle);
        controller.listAlienInRange.Remove(other.transform.parent.GetComponent<Alien>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public CharacterBase controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            controller.listAlienInRange.Add(other.transform.parent.GetComponent<EnemyBase>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //other.transform.parent.GetComponent<Alien>().ChangeState(AlienState.Idle);
            controller.listAlienInRange.Remove(other.transform.parent.GetComponent<EnemyBase>());
            other.transform.parent.GetComponent<EnemyBase>().Escaped(controller);
        }
    }
}

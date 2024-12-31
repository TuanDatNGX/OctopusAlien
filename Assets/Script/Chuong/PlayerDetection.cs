using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public CharacterBase controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject!=controller.model.gameObject)
        {
            controller.listTargets.Add(other.transform.parent.GetComponent<TargetBase>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject != controller.gameObject)
        {           
            controller.listTargets.Remove(other.transform.parent.GetComponent<TargetBase>());
            other.transform.parent.GetComponent<TargetBase>().Escaped(controller);
        }
    }
}

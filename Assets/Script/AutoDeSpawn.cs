using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeSpawn : MonoBehaviour
{
    [SerializeField] float time;

    private void OnEnable()
    {
        StartCoroutine(DelayDespawn());
    }

    IEnumerator DelayDespawn()
    {
        yield return new WaitForSeconds(time);
        LeanPool.Despawn(gameObject);
    }
}

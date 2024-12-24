using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject bloodFx;
    public GameObject bloodFx2;

    public void SpawnBloodFx(Vector3 pos)
    {
        GameObject fx = LeanPool.Spawn(bloodFx, transform);
        fx.transform.position = pos;
        LeanPool.Despawn(fx, 1f);
    }

    public void SpawnBloodFx2(Vector3 pos)
    {
        GameObject fx = LeanPool.Spawn(bloodFx2, transform);
        fx.transform.position = pos;
        LeanPool.Despawn(fx, 3);
    }
}

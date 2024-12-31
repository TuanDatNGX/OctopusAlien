using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class TargetBase : MonoBehaviour
{
    public bool isOctopus;
    public int rank;
    public List<GameObject> listAttacker;
    public Transform hit;
    public bool isDie;
    public GameObject growingRoot;
    public float hpNow;
    public HpBarController hpBar;
    public Transform posHpBar;
    public abstract bool TakeDamage(OctopusTail _octopusTail);
    public abstract void Die();
    public abstract void AffterDie(CharacterBase _octopus=null);
    public abstract void UpdateHp(float _value, OctopusTail _octopusTail = null);
    public abstract void Escaped(CharacterBase _characterBase);
}

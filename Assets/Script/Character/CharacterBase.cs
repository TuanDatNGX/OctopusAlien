using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum StateCharacter
{
    Idle,
    Attack
}

[Serializable]
public struct CharacterStatsBase
{
    public string id;
    public float hp;
    public float attack;
    public float delayAttack;
    public float rangeAttack;
    public float moveSpeed;
    public float levelAI;
}

public abstract class CharacterBase : MonoBehaviour
{
    public CharacterStatsBase characterStatsBase;
    public StateCharacter stateNow;
    public OctopusTail[] tails;
    public List<Alien> listAlienInRange;

    public abstract void Move();
    public abstract void Attack();

    private void Update()
    {
        Move();
        switch (stateNow)
        {
            case StateCharacter.Idle:
                break;
            case StateCharacter.Attack:
                break;
        }
    }

    public void ChangeState(StateCharacter _stateCharacter)
    {
        if (_stateCharacter == stateNow) return;
        switch (_stateCharacter)
        {
            case StateCharacter.Idle:
                break;
            case StateCharacter.Attack:
                break;
        }
    }
}

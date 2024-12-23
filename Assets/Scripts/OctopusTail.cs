using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TailState
{
    Idle,
    Catch,
    Collect,
}

public class OctopusTail : MonoBehaviour
{
    public PlayerController player;
    public TailAnimator2 tailAnimator;
    public TailState currentState = TailState.Idle;
    public Alien target;
    Vector3 defaultRotation;

    private void Awake()
    {
        defaultRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        if (currentState == TailState.Catch)
        {
            transform.LookAt(target.transform);
        }
    }

    public void ChangeState(TailState state)
    {
        if (currentState == state) return;
        currentState = state;
        switch(state)
        {
            case TailState.Idle:
                target = null;
                tailAnimator.UseIK = false;
                tailAnimator.IKTarget = null;
                tailAnimator.TailAnimatorAmount = 1f;
                transform.DOLocalRotate(defaultRotation, 0.25f);
                break;
            case TailState.Catch:
                tailAnimator.UseIK = true;
                tailAnimator.TailAnimatorAmount = 0.05f;
                break;
            case TailState.Collect:break;
        }
    }

    public void CatchAlien(Alien alien)
    {
        target = alien;
        alien.target = this;
        tailAnimator.IKTarget = alien.transform;
        alien.ChangeState(AlienState.Catched);
        ChangeState(TailState.Catch);
    }
}

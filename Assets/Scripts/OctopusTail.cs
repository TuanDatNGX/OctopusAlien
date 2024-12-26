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
    float currentBlend = 0;

    private void Awake()
    {
        defaultRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        if (currentState == TailState.Catch)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
        switch (currentState)
        {
            case TailState.Idle:
                break;
            case TailState.Catch:
                currentBlend += 4 * Time.deltaTime;
                if (currentBlend >= 1) currentBlend = 1;
                tailAnimator.IKBlend = currentBlend;
                break;
            case TailState.Collect: 
                break;
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
                tailAnimator.Slithery = 0.67f;
                transform.DOLocalRotate(defaultRotation, 0.25f);
                break;
            case TailState.Catch:
                tailAnimator.UseIK = true;
                tailAnimator.TailAnimatorAmount = 0.25f;
                currentBlend = 0;
                tailAnimator.IKBlend = 0;
                tailAnimator.IKContinousSolve = true;
                tailAnimator.Slithery = 1f;
                break;
            case TailState.Collect:break;
        }
    }

    public void CatchAlien(Alien alien)
    {
        target = alien;
        alien.target = this;
        tailAnimator.IKTarget = alien.hit;
        alien.ChangeState(AlienState.Catched);
        ChangeState(TailState.Catch);
    }
}

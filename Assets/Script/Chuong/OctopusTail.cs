using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TailState
{
    None,
    Idle,
    Catch,
    Collect,
}

public class OctopusTail : MonoBehaviour
{
    public CharacterBase octopus;
    public TailAnimator2 tailAnimator;
    public TailState currentState = TailState.None;
    public TargetBase target;
    public ParticleSystem[] effectTail;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    Vector3 defaultRotation;
    float currentBlend = 0;
    float speedCollect = 5;
    bool canCatch = false;
    Material defaultMat;
    float defaultSpeedCollect;

    private void Awake()
    {
        defaultRotation = transform.localEulerAngles;
        defaultSpeedCollect = speedCollect;
        ChangeState(TailState.Idle);
    }

    private void OnEnable()
    {
        Material[] materials = new Material[2];

        defaultMat = skinnedMeshRenderer.material;
        skinnedMeshRenderer.material = octopus.outlineMat;
        tailAnimator.LengthMultiplier = 0;
        StartCoroutine(ActiveMesh());
    }

    IEnumerator ActiveMesh()
    {
        yield return new WaitForSeconds(0.01f);
        skinnedMeshRenderer.enabled = true;
        DOTween.To(() => tailAnimator.LengthMultiplier, x => tailAnimator.LengthMultiplier = x, 1f, 0.7f).OnComplete(() =>
        {
            skinnedMeshRenderer.material = defaultMat;
            foreach (var effect in effectTail)
            {
                effect.Play();
            }
            canCatch = true;
        });
    }

    private void Update()
    {
        //if (currentState == TailState.Catch)
        //{
        //    transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        //}
        switch (currentState)
        {
            case TailState.Idle:
                break;
            case TailState.Catch:
                CheckTargetOutRange();
                if (target != null)
                {
                    if (target.TakeDamage(this))
                    {
                        if (!tailAnimator.IKTarget)
                        {
                            transform.DORewind(true);
                            tailAnimator.UseIK = true;
                            tailAnimator.IKTarget = target.hit;
                            tailAnimator.TailAnimatorAmount = 0.85f;
                            currentBlend = 0;
                            tailAnimator.IKBlend = 0;
                            tailAnimator.IKContinousSolve = true;
                            tailAnimator.Slithery = 1f;
                        }
                        currentBlend += 4 * Time.deltaTime;
                        if (currentBlend >= 1) currentBlend = 1;
                        tailAnimator.IKBlend = currentBlend;
                        if (octopus.stateNow == StateCharacter.Die)
                        {
                            target.Escaped(octopus);
                            ChangeState(TailState.Idle);
                        }
                    }
                }
                break;
            case TailState.Collect:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == TailState.Catch)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
    }

    public void ChangeState(TailState state)
    {
        if (currentState == state) return;
        currentState = state;
        switch (state)
        {
            case TailState.Idle:
                target = null;
                tailAnimator.UseIK = false;
                tailAnimator.IKTarget = null;
                tailAnimator.TailAnimatorAmount = 1f;
                tailAnimator.Slithery = 1;
                transform.DOLocalRotate(defaultRotation, 2);
                break;
            case TailState.Catch:
                //tailAnimator.UseIK = true;
                //tailAnimator.TailAnimatorAmount = 0.85f;
                //currentBlend = 0;
                //tailAnimator.IKBlend = 0;
                //tailAnimator.IKContinousSolve = true;
                //tailAnimator.Slithery = 1f;
                break;
            case TailState.Collect:
                tailAnimator.TailAnimatorAmount = 0.15f;
                break;
        }
    }

    public void CatchTarget(TargetBase _target)
    {
        target = _target;
        ChangeState(TailState.Catch);
    }

    void CheckTargetOutRange()
    {
        if (!octopus.listTargets.Contains(target) || target.isDie)
        {
            if (currentState != TailState.Collect)
            {
                octopus.listTargets.Remove(target);
                target = null;
                ChangeState(TailState.Idle);
            }
        }
    }

    public void CollectTarget()
    {
        octopus.listTargets.Remove(target);
        ChangeState(TailState.Collect);
        StartCoroutine(CollectingTarget());
    }

    IEnumerator CollectingTarget()
    {
        octopus.ActionEat();
        while (true)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, octopus.mouth.position, speedCollect * Time.deltaTime);
            if(Vector3.Distance(target.transform.position, octopus.mouth.position) < .1f)
            {
                speedCollect = defaultSpeedCollect;
                octopus.killCount++;
                target.AffterDie(octopus);
                EffectController.Instance.SpawnBloodFx(target.transform.position);
                AudioManager.Instance.PlaySoundEat();
                target = null;
                ChangeState(TailState.Idle);
                break;
            }

            yield return null;
        }
    }
}

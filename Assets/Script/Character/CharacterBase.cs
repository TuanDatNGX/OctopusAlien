using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.AI;

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
    public float tentacle;
    public float heal;
}

public abstract class CharacterBase : MonoBehaviour
{
    public bool isBot;
    public CharacterStatsBase characterStatsBase;
    public LevelDataAssetsSO levelUpData;
    public Animator faceAnim;
    public float moveSpeed;
    public float rotateSpeed;
    public OctopusTail[] tails;
    public List<EnemyBase> listAlienInRange;
    public Transform model;
    public Transform rangeObj;
    public Transform mouth;
    public GameObject rangeZone;
    public ParticleSystem levelUpFx;
    public ParticleSystem levelUpFx2;
    public Material mat2;
    public Material mat3;
    public Material matFace2;
    public Material matFace3;
    public Material outlineMat;
    public EnemyBase neareastAlien;
    public Coroutine rangeActive;
    public float currentExp = 0;
    public int currentLevel = 1;
    public float defaultScale;
    public float currentEvol = 0;
    public float defaultMovespeed = 0;
    public int killCount = 0;
    float defaultCam;
    public StateCharacter stateNow;


    private void Awake()
    {
        defaultScale = model.transform.localScale.x;
        //defaultMovespeed = characterStatsBase.moveSpeed;
        moveSpeed = characterStatsBase.moveSpeed;
        if (!isBot)
        {
            defaultCam = Camera.main.fieldOfView;
        }
    }

    public abstract void Move();
    public abstract void Attack();
    public abstract void LevelUp();

    private void Update()
    {
        neareastAlien = null;
        rangeObj.transform.position = new Vector3(rangeObj.transform.position.x, transform.position.y - 0.65f, rangeObj.transform.position.z);
        foreach (var alien in listAlienInRange)
        {
            float minDistance = 1000;
            if (!alien.listAttacker.Contains(gameObject))
            {
                float distance = Vector3.Distance(transform.position, alien.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    neareastAlien = alien;
                }
            }
        }
        foreach (var tail in tails)
        {
            if (tail.gameObject.activeSelf && tail.currentState == TailState.Idle)
            {
                if (neareastAlien != null)
                {
                    if (!neareastAlien.listAttacker.Contains(gameObject))
                    {
                        tail.CatchAlien(neareastAlien);
                    }
                    break;
                }
            }
        }
        if (listAlienInRange.Count > 0)
        {
            rangeZone.SetActive(true);
            if (rangeActive != null)
            {
                StopCoroutine(rangeActive);
                rangeActive = null;
            }
        }
        else
        {
            if (rangeActive == null)
                rangeActive = StartCoroutine(DeActiveRangeZone());
        }
        Move();

        //Move();
        //switch (stateNow)
        //{
        //    case StateCharacter.Idle:
        //        break;
        //    case StateCharacter.Attack:
        //        break;
        //}

        if (Input.GetKeyDown(KeyCode.C))
        {
            if(!isBot) 
            {
                GetExp(levelUpData.enemyAssets[currentLevel.ToString()].exp);
            }
        }
    }

    IEnumerator DeActiveRangeZone()
    {
        yield return new WaitForSeconds(1f);
        rangeZone.SetActive(false);
        rangeActive = null;
    }

    public void GetExp(float _exp)
    {
        if (currentLevel >= 20) return;
        currentExp += _exp;
        while (currentExp >= levelUpData.enemyAssets[currentLevel.ToString()].exp)
        {
            currentExp -= levelUpData.enemyAssets[currentLevel.ToString()].exp;
            currentLevel += 1;
            levelUpFx.Play();
            if (levelUpData.enemyAssets[currentLevel.ToString()].size != 0)
            {
                model.transform.DOScale(model.localScale.x + defaultScale * levelUpData.enemyAssets[currentLevel.ToString()].size, 0.35f);
                if (!isBot)
                {
                    Camera.main.DOFieldOfView(Camera.main.fieldOfView + 0.4f * defaultCam * levelUpData.enemyAssets[currentLevel.ToString()].size, 0.35f);
                    Camera.main.transform.DOLocalMoveY(Camera.main.transform.position.y + 1.67f * levelUpData.enemyAssets[currentLevel.ToString()].size, 0.35f);
                }
            }
            if (levelUpData.enemyAssets[currentLevel.ToString()].tentacles > 0)
            {
                for (int i = 0; i < tails.Length; i++)
                {
                    if (!tails[i].gameObject.activeSelf)
                    {
                        tails[i].gameObject.SetActive(true);
                        levelUpFx2.Play();
                        return;
                    }
                }
            }
            if (levelUpData.enemyAssets[currentLevel.ToString()].CatchingRadius != 0)
            {
                //rangeObj.transform.DOScale(rangeObj.localScale.x + 0.2f * levelUpData.enemyAssets[currentLevel.ToString()].CatchingRadius, 0.35f);
                //foreach (OctopusTail t in tails)
                //{
                //    t.tailAnimator.LengthMultiplier = t.tailAnimator.LengthMultiplier + 0.05f * levelUpData.enemyAssets[currentLevel.ToString()].CatchingRadius;
                //}
            }
            if (levelUpData.enemyAssets[currentLevel.ToString()].evol != 0)
            {
                if (levelUpData.enemyAssets[currentLevel.ToString()].evol == 1)
                {
                    model.GetComponent<MeshRenderer>().material = matFace2;
                    foreach (var item in tails)
                    {
                        item.tailAnimator.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = mat2;
                    }
                }

                else if (levelUpData.enemyAssets[currentLevel.ToString()].evol == 2)
                {
                    model.GetComponent<MeshRenderer>().material = matFace3;
                    foreach (var item in tails)
                    {
                        item.tailAnimator.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = mat3;
                    }
                }
                levelUpFx2.Play();
            }
            characterStatsBase.attack += levelUpData.enemyAssets[currentLevel.ToString()].Atk;
            characterStatsBase.hp += levelUpData.enemyAssets[currentLevel.ToString()].Hp;
            characterStatsBase.heal += levelUpData.enemyAssets[currentLevel.ToString()].Heal;
            characterStatsBase.moveSpeed += levelUpData.enemyAssets[currentLevel.ToString()].speed;
            characterStatsBase.rangeAttack += levelUpData.enemyAssets[currentLevel.ToString()].CatchingRadius;
            moveSpeed = characterStatsBase.moveSpeed;
            Handheld.Vibrate();
            LevelUp();
        }
        if (!isBot)
        {
            UiController.Instance.UpdateExp(currentExp, levelUpData.enemyAssets[currentLevel.ToString()].exp, currentLevel, killCount);
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
    public void ActionEat()
    {
        Eat = StartCoroutine(CoEat());
    }

    Coroutine Eat;
    IEnumerator CoEat()
    {
        yield return new WaitForSeconds(0.1f);
        faceAnim.Play("Eat");
    }
}

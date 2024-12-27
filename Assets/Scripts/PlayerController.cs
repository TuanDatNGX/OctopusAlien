using DG.Tweening;
using FIMSpace.FTail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LevelDataAssetsSO levelUpData;
    public CharacterController characterController;
    public CharacterStat characterStat;
    public FloatingJoystick joystick;
    public Animator faceAnim;
    public float moveSpeed;
    public float rotateSpeed;
    Vector3 inputDirection;
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
    EnemyBase neareastAlien;
    Coroutine rangeActive;
    float currentExp = 0;
    int currentLevel = 1;
    float defaultScale;
    float defaultCam;
    float currentEvol = 0;
    float defaultMovespeed = 0;

    private void Start()
    {
        UiController.Instance.UpdateExp(0, levelUpData.enemyAssets[currentLevel.ToString()].exp, currentLevel);
        defaultScale = model.transform.localScale.x;
        defaultCam = Camera.main.fieldOfView;
        defaultMovespeed = moveSpeed;
    }

    private void Update()
    {
        neareastAlien = null;
        rangeObj.transform.position = new Vector3(rangeObj.transform.position.x, transform.position.y - 0.65f, rangeObj.transform.position.z);
        foreach (var alien in listAlienInRange)
        {
            float minDistance = 1000;
            if (alien.stateNow == State.Idle)
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
            if(tail.gameObject.activeSelf && tail.currentState == TailState.Idle)
            {
                if(neareastAlien != null)
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
             if(rangeActive == null)
             rangeActive = StartCoroutine(DeActiveRangeZone());
        }
        Move();

        if(Input.GetKeyDown(KeyCode.C))
        {
            AudioManager.Instance.PlaySoundLevelUp();
            for (int i = 0; i < tails.Length; i++)
            {
                if (!tails[i].gameObject.activeSelf)
                {
                    tails[i].skinnedMeshRenderer.enabled = false;
                    tails[i].gameObject.SetActive(true);
                    levelUpFx2.Play();
                    return;
                }
            }
        }
    }

    IEnumerator DeActiveRangeZone()
    {
        yield return new WaitForSeconds(1f);
        rangeZone.SetActive(false);
        rangeActive = null;
    }

    public void GetExp(float exp)
    {
        if (currentLevel >= 20) return;
        currentExp += exp;
        if(currentExp >= levelUpData.enemyAssets[currentLevel.ToString()].exp)
        {
            AudioManager.Instance.PlaySoundLevelUp();
        }
        while (currentExp >= levelUpData.enemyAssets[currentLevel.ToString()].exp)
        {
            currentExp -= levelUpData.enemyAssets[currentLevel.ToString()].exp;
            currentLevel += 1;
            levelUpFx.Play();
            if (levelUpData.enemyAssets[currentLevel.ToString()].size != 0)
            {
                model.transform.DOScale(model.localScale.x + defaultScale * levelUpData.enemyAssets[currentLevel.ToString()].size, 0.35f);
                Camera.main.DOFieldOfView(Camera.main.fieldOfView + 0.4f * defaultCam * levelUpData.enemyAssets[currentLevel.ToString()].size, 0.35f);
            }
            if (levelUpData.enemyAssets[currentLevel.ToString()].tentacles > 0)
            {
                for (int i = 0; i< tails.Length; i++)
                {
                    if (!tails[i].gameObject.activeSelf)
                    {
                        tails[i].skinnedMeshRenderer.enabled = false;
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
                if(levelUpData.enemyAssets[currentLevel.ToString()].evol == 1)
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
            characterStat.ATK += levelUpData.enemyAssets[currentLevel.ToString()].Atk;
            characterStat.HP += levelUpData.enemyAssets[currentLevel.ToString()].Hp;
            characterStat.Heal += levelUpData.enemyAssets[currentLevel.ToString()].Heal;
            characterStat.MoveSpeed += levelUpData.enemyAssets[currentLevel.ToString()].speed;
            characterStat.Radius += levelUpData.enemyAssets[currentLevel.ToString()].CatchingRadius;
            characterStat.Tentacle += levelUpData.enemyAssets[currentLevel.ToString()].tentacles;

            moveSpeed = defaultMovespeed * characterStat.MoveSpeed / 10;

            StatsBoard.Instance.UpdateData(characterStat);
        }
        UiController.Instance.UpdateExp(currentExp, levelUpData.enemyAssets[currentLevel.ToString()].exp, currentLevel);
    }

    protected void Move()
    {
        inputDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        if (inputDirection.magnitude > 0.1f)
        {
            UiController.Instance.dragToMove.gameObject.SetActive(false);
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            characterController.Move(inputDirection.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move(Vector3.zero * Time.deltaTime);
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
        AudioManager.Instance.PlaySoundEat();
        faceAnim.Play("Eat");
    }
}

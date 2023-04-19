using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public BoxCollider boxCollider;
    public float hp = 100;
    public MoveState currentMoveState = MoveState.Walking;
    public HumanState currentHumanState = HumanState.Normal;
    public FunctionalSpace currentPlace;
    public Slider hpSlider;
    public float walkSpeed = 2f;
    public float eventsDelayMin = 3;
    public float eventsDelayMax = 10;
    public float hpfallspeed = 4;
    public float hpbarhidedelay = 3;
    public float hpregspeed = 4;
    public Animator animator;
    public GameObject OnePlusEffectPrefab;
    public float moneyFrequency = 2;

    public GameObject spriteNormal;
    public GameObject spriteOnFire;
    public GameObject spriteFainted;

    private bool isWalkingRight = true;
    private float walkMin;
    private float walkMax;

    private float maxHp;
    float eventDelay;
    float timerToEvent = 0;

    public Transform spritesHolderTransform;
    public GameObject poofEffectPrefab;

    public enum MoveState
    {
        Walking,
        Idle,
        Regenerating,
        Waiting,
        Dragged
    }

    public enum HumanState
    {
        Normal,
        OnFire,
        Fainted
    }

    IEnumerator MoneyMakingRoutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, 2));
        while (true)
        {
            yield return new WaitForSeconds(moneyFrequency);

            if (currentHumanState == HumanState.Normal && currentMoveState == MoveState.Walking)
            {
                MoneyManager.instance.AddCoin();
                Destroy(Instantiate(OnePlusEffectPrefab, transform.position, Quaternion.identity), 1.5f);
            }
        }
    }

    private void Start()
    {
        maxHp = hp;
        DroppedOn(currentPlace);
        ResetEventDelay();
        StartCoroutine(MoneyMakingRoutine());
        if (MoneyManager.instance == null) return;

        MoneyManager.instance.IncreasePeople();
    }

    private void ResetEventDelay()
    {
        if (MoneyManager.instance == null)
        {
            eventDelay = UnityEngine.Random.Range(eventsDelayMin, eventsDelayMax);
            timerToEvent = 0;
            return;
        }
        float a = Mathf.Lerp(1, 4.5f, TempManager.instance.temp / TempManager.instance.maxTemp);
        eventDelay = UnityEngine.Random.Range(eventsDelayMin / a, eventsDelayMax / a);
        timerToEvent = 0;
    }

    public void EnteredDrag()
    {
        SetMoveStateTo(MoveState.Dragged);
    }

    public void DroppedOn(FunctionalSpace newPlace)
    {
        if (newPlace == null) return;

        transform.parent = newPlace.transform;

        if (this.currentPlace != null)
        {
            this.currentPlace.UpdateText();
            this.currentPlace = newPlace;
        }
        

        transform.position = newPlace.GetHumanPos(transform.position);

        if (currentHumanState == HumanState.Normal && newPlace.roomType == FunctionalSpace.RoomType.WalkFloor)
        {
            walkMin = newPlace.gameObject.GetComponent<Floor>().minRange;
            walkMax = newPlace.gameObject.GetComponent<Floor>().maxRange;
            SetMoveStateTo(MoveState.Walking);
        }

        if (newPlace.roomType == FunctionalSpace.RoomType.WaitingRoom)
        {
            SetMoveStateTo(MoveState.Waiting);
        }

        if (newPlace.roomType == FunctionalSpace.RoomType.Nursery)
        {
            SetMoveStateTo(MoveState.Regenerating);
            if (currentHumanState == HumanState.Fainted)
            {
                SetHumanStateTo(HumanState.Normal);
            }
        }

        if (newPlace.roomType == FunctionalSpace.RoomType.SwimPool)
        {
            SetMoveStateTo(MoveState.Regenerating);
            if (currentHumanState == HumanState.OnFire)
            {
                SetHumanStateTo(HumanState.Normal);
            }
        }
    }

    private void Update()
    {
        if (MoneyManager.instance == null) return;
        if (!MoneyManager.instance.gameInProgress) return;

        if (currentMoveState == MoveState.Walking)
        {
            HandleTimeToDelayUpdate();
            if (isWalkingRight)
            {
                spritesHolderTransform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.right * walkSpeed * Time.deltaTime;
                if (transform.position.x >= walkMax)
                {
                    isWalkingRight = !isWalkingRight;
                }
            }
            else
            {
                spritesHolderTransform.localScale = new Vector3(-1, 1, 1);
                transform.position -= Vector3.right * walkSpeed * Time.deltaTime;
                if (transform.position.x <= walkMin)
                {
                    isWalkingRight = !isWalkingRight;
                }
            }
        }

        if (currentMoveState == MoveState.Waiting)
        {
            if (hp == maxHp)
            {
                return;
            }
            hp = Mathf.Clamp(hp - hpfallspeed / 4.5f * Time.deltaTime, 0, maxHp);
            UpdateHP();
        }
        else if (currentHumanState == HumanState.OnFire || currentHumanState == HumanState.Fainted)
        {
            if (currentMoveState != MoveState.Dragged)
            {
                hp = Mathf.Clamp(hp - hpfallspeed * Time.deltaTime, 0, maxHp);
                UpdateHP();
            }
        }

        if (currentMoveState == MoveState.Regenerating)
        {
            if (hp == maxHp)
            {
                ClickManager.instance.SendBackToRandomFloor(this);
                return;
            }
            hp = Mathf.Clamp(hp + hpregspeed * Time.deltaTime, 0, maxHp);
            UpdateHP();
        }

        

        if (hp == 0)
        {
            BeforeDestroy();
            Destroy(gameObject);
        }
    }

    private void HandleTimeToDelayUpdate()
    {
        timerToEvent += Time.deltaTime;
        if(timerToEvent >= eventDelay)
        {
            ResetEventDelay();
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                SetMoveStateTo(MoveState.Idle);
                SetHumanStateTo(HumanState.OnFire);
            }
            else
            {
                SetMoveStateTo(MoveState.Idle);
                SetHumanStateTo(HumanState.Fainted);
            }
        }
    }

    public void SetMoveStateTo(MoveState state)
    {
        isWalkingRight = UnityEngine.Random.Range(0, 2) == 1;
        currentMoveState = state;
        switch (state)
        {
            case MoveState.Walking:
                animator.Play("Walk");
                break;
            case MoveState.Idle:
                animator.Play("Idle");
                break;
            case MoveState.Regenerating:
                animator.Play("Idle");
                break;
            case MoveState.Waiting:
                animator.Play("Idle");
                break;
        }
    }

    public void SetHumanStateTo(HumanState state)
    {
        currentHumanState = state;
        switch (state)
        {
            case HumanState.Normal:
                SetColliderZSizeTo(1);
                SetSpriteTo(spriteNormal);
                break;
            case HumanState.OnFire:
                SetColliderZSizeTo(2);
                SetSpriteTo(spriteOnFire);
                animator.Play("OnFire");
                break;
            case HumanState.Fainted:
                SetColliderZSizeTo(2);
                SetSpriteTo(spriteFainted);
                animator.Play("Fainted");
                break;
        }
    }

    private void SetColliderZSizeTo(float newZ)
    {
        var newSize = transform.localScale;
        newSize.z = newZ;
        transform.localScale = newSize;
    }

    Coroutine hidebarrotuine = null;

    public void UpdateHP()
    {
        hpSlider.value = hp / maxHp;
        hpSlider.gameObject.SetActive(true);

        if (hidebarrotuine != null)
            StopCoroutine(hidebarrotuine);
        hidebarrotuine = StartCoroutine(HideHPBarAfterDelay());
    }

    public IEnumerator HideHPBarAfterDelay()
    {
        yield return new WaitForSeconds(hpbarhidedelay);
        hpSlider.gameObject.SetActive(false);
    }

    private void SetSpriteTo(GameObject spriteObject)
    {
        spriteNormal.SetActive(false);
        spriteFainted.SetActive(false);
        spriteOnFire.SetActive(false);
        spriteObject.SetActive(true);
    }

    private void BeforeDestroy()
    {
        MoneyManager.instance.DecreasePeople();
        if (currentPlace != null)
        {
            currentPlace.UpdateText();
        }
        Destroy(Instantiate(poofEffectPrefab, transform.position, Quaternion.identity), 1.5f);
    }
}

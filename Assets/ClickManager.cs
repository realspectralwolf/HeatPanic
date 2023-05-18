using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Human;
using DG.Tweening;
using TMPro;

public class ClickManager : MonoBehaviour
{
    public FunctionalSpace[] availableFloors;

    public LayerMask humanLayer;
    public LayerMask placeLayer;

    [SerializeField] GameObject vfx_wrongRoom;

    public Human heldHuman = null;
    private Vector3 initialHumanPos;
    private MoveState initialMoveState;

    public bool isMouseOverHuman = false;

    public static System.Action RoomFull;

    public static ClickManager instance;

    public System.Action PickedUpHuman;
    public System.Action CanceledHumanMove;

    public void Awake()
    {
        instance = this;
    }

    public void SendBackToRandomFloor(Human targetHuman)
    {
        var newPos = targetHuman.transform.position;
        var targetFloor = availableFloors[UnityEngine.Random.Range(0, availableFloors.Length)];
        newPos.y = targetFloor.transform.position.y;
        newPos.x = 2;
        targetHuman.transform.DOMove(newPos, 0.2f).onComplete += () => { heldHuman.SetMoveStateTo(MoveState.Walking); }; // Set the easing function
        targetHuman.DroppedOn(targetFloor);
    }

    private void Update()
    {
        bool isInputDisabled;

        if (MoneyManager.instance == null)
            isInputDisabled = false;
        else
            isInputDisabled = !MoneyManager.instance.gameInProgress;

        if (isInputDisabled) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        isMouseOverHuman = Physics.Raycast(ray, out hit, Mathf.Infinity, humanLayer);

        if (Input.GetMouseButtonDown(0))
        {
            if (isMouseOverHuman)
            {
                heldHuman = hit.collider.transform.parent.parent.gameObject.GetComponent<Human>();
                initialHumanPos = heldHuman.transform.position;
                initialMoveState = heldHuman.currentMoveState;
                heldHuman.EnteredDrag();
                PickedUpHuman?.Invoke();
            }
            else
            {
                CursorUI.instance.SpawnClickEffect();
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (heldHuman == null)
                return;

            RaycastHit hit2;

            if (Physics.Raycast(ray, out hit2, Mathf.Infinity, placeLayer))
            {
                var targetRoom = hit2.collider.gameObject.GetComponent<FunctionalSpace>();
                bool cond1 = targetRoom.roomType == FunctionalSpace.RoomType.SwimPool && heldHuman.currentHumanState == HumanState.OnFire;
                bool cond2 = targetRoom.roomType == FunctionalSpace.RoomType.Nursery && heldHuman.currentHumanState == HumanState.Fainted;
                bool cond3 = targetRoom.roomType == FunctionalSpace.RoomType.WaitingRoom && (heldHuman.currentHumanState == HumanState.Fainted || heldHuman.currentHumanState == HumanState.OnFire);
                bool cond4 = targetRoom.roomType == FunctionalSpace.RoomType.WalkFloor && heldHuman.currentHumanState == HumanState.Normal;

                if (!targetRoom.HasFreeSpace())
                {
                    RoomFull?.Invoke();
                }

                if (targetRoom.HasFreeSpace() && (cond1 || cond2 || cond3 || cond4))
                {
                    heldHuman.DroppedOn(targetRoom);
                }
                else
                {
                    if (((heldHuman.currentHumanState == HumanState.OnFire && targetRoom.roomType == FunctionalSpace.RoomType.Nursery) 
                        || (heldHuman.currentHumanState == HumanState.Fainted && targetRoom.roomType == FunctionalSpace.RoomType.SwimPool))
                        && !heldHuman.isTutorialOnly)
                    {
                        Destroy(Instantiate(vfx_wrongRoom, heldHuman.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity), 2);
                        heldHuman.Kill();
                    }
                    else
                    {
                        ResetHumanToInitialPos();
                        CanceledHumanMove?.Invoke();
                    }
                }
            }
            else
            {
                ResetHumanToInitialPos();
                CanceledHumanMove?.Invoke();
            }
            

            heldHuman = null;
        }

        if (heldHuman != null)
        {
            Vector3 mousePosScreen = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
            mousePosWorld.z = 0;

            heldHuman.transform.position = mousePosWorld;
        }
    }

    void ResetHumanToInitialPos()
    {
        var tempHuman = heldHuman;
        var tempState = initialMoveState;
        DOTween.Kill(transform);
        tempHuman.transform.DOMove(initialHumanPos, 0.2f).OnComplete(() => { if (tempHuman != null) tempHuman.SetMoveStateTo(tempState); });
    }
}

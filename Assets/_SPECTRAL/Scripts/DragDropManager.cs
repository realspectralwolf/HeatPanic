using System.Collections.Generic;
using UnityEngine;

public class DragDropManager : MonoBehaviour
{
    public LayerMask humanLayer;
    public LayerMask placeLayer;

    public Human heldHuman = null;
    private Vector3 initialHumanPos;
    private HumanState initialState;

    public bool isMouseOverHuman = false;

    public static System.Action RoomFull;

    public static DragDropManager Instance;

    public System.Action PickedUpHuman;
    public System.Action CanceledHumanMove;

    private List<Location> availableFloors = new();

    public void Awake()
    {
        Instance = this;

        var locations = FindObjectsOfType<Location>();
        foreach (var location in locations)
        {
            if (location.roomType == Location.RoomType.WalkFloor)
            {
                availableFloors.Add(location);
            }
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        isMouseOverHuman = Physics.Raycast(ray, out hit, Mathf.Infinity, humanLayer);

        if (Input.GetMouseButtonDown(0))
        {
            if (isMouseOverHuman)
            {
                heldHuman = hit.collider.transform.parent.parent.gameObject.GetComponent<Human>();
                initialHumanPos = heldHuman.transform.position;
                initialState = heldHuman.CurrentState;
                heldHuman.PickedUp();
                PickedUpHuman?.Invoke();
            }
            else
            {
                CursorUI.instance.SpawnClickEffect();
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            HandleHumanDrop(heldHuman, ray);
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

    void HandleHumanDrop(Human humanToDrop, Ray ray)
    {
        if (humanToDrop == null)
            return;

        RaycastHit hit2;
        if (!Physics.Raycast(ray, out hit2, Mathf.Infinity, placeLayer))
        {
            ResetHumanToInitialPos();
            CanceledHumanMove?.Invoke();
            return;
        }

        var targetRoom = hit2.collider.gameObject.GetComponent<Location>();

        if (targetRoom.DoesAcceptState(heldHuman.CurrentState))
        {
            if (targetRoom.HasFreeSpace())
            {
                humanToDrop.DroppedOn(targetRoom);
                return;
            }
            else
            {
                RoomFull?.Invoke();
            }
        }
        
        if (targetRoom.DoesInstaKillState(heldHuman.CurrentState) && !humanToDrop.IsTutorialOnly)
        {
            Destroy(Instantiate(DataHolder.Instance.GameData.vfx_wrongRoom, humanToDrop.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity), 2);
            humanToDrop.Kill();
        }

        ResetHumanToInitialPos();
        CanceledHumanMove?.Invoke();
    }

    void ResetHumanToInitialPos()
    {
        var tempHuman = heldHuman;
        var tempState = initialState;
        LeanTween.cancel(transform.gameObject);
        tempHuman.transform.LeanMove(initialHumanPos, 0.2f).setOnComplete(() => { if (tempHuman != null) tempHuman.SetState(tempState); });
    }

    public void SendBackToRandomFloor(Human targetHuman)
    {
        var newPos = targetHuman.transform.position;
        var targetFloor = availableFloors[Random.Range(0, availableFloors.Count)];
        newPos.y = targetFloor.transform.position.y;
        newPos.x = 2;
        targetHuman.transform.LeanMove(newPos, 0.2f).setOnComplete(() => { targetHuman.SetState(targetHuman.HumanData.walkState); }); // Set the easing function
        targetHuman.DroppedOn(targetFloor);
        targetHuman.SetState(targetHuman.HumanData.walkState);
    }
}

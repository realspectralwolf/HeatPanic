using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsHint : MonoBehaviour
{
    [SerializeField] GameObject hintPanel;

    int timesDroppedOnWaitingRoom = 0;

    private void OnEnable()
    {
        Human.OnDroppedOn += HandleHumanDroppedOn;
        DragDropManager.RoomFull += HandleNotEnoughSpace;
    }

    private void OnDisable()
    {
        Human.OnDroppedOn -= HandleHumanDroppedOn;
        DragDropManager.RoomFull -= HandleNotEnoughSpace;
    }

    private void HandleHumanDroppedOn(Location space)
    {
        if (space.roomType == Location.RoomType.WaitingRoom)
        {
            timesDroppedOnWaitingRoom++;
            gameObject.SetActive(false);
        }
    }

    private void HandleNotEnoughSpace()
    {
        if (timesDroppedOnWaitingRoom == 0)
        {
            hintPanel.SetActive(true);
        }
    }
}

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
        ClickManager.RoomFull += HandleNotEnoughSpace;
    }

    private void OnDisable()
    {
        Human.OnDroppedOn -= HandleHumanDroppedOn;
        ClickManager.RoomFull -= HandleNotEnoughSpace;
    }

    private void HandleHumanDroppedOn(FunctionalSpace space)
    {
        if (space.roomType == FunctionalSpace.RoomType.WaitingRoom)
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

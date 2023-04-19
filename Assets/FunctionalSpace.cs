using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;

public class FunctionalSpace : MonoBehaviour
{
    [SerializeField] public RoomType roomType;
    [SerializeField] float maxHumans = 3;
    [SerializeField] TextMeshProUGUI textCapacity;
    [SerializeField] GameObject noFreeSlotsObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool HasFreeSpace()
    {
        bool state = transform.childCount < maxHumans;
        if (!state)
        {
            noFreeSlotsObject.SetActive(true);
        }
        return (state);
    }

    public Vector3 GetHumanPos(Vector3 humanPos)
    {
        switch (roomType)
        {
            case (RoomType.WalkFloor):
                humanPos.y = transform.position.y;
                break;
            case (RoomType.SwimPool):
                humanPos.y = -1.74f;
                humanPos.x = transform.position.x - 2f + transform.childCount * 0.66f;
                break;
            case (RoomType.Nursery):
                humanPos.y = -0.12f;
                humanPos.x = transform.position.x - 0.61f + transform.childCount * 1f;
                break;
            case (RoomType.WaitingRoom):
                humanPos.y = transform.position.y;
                humanPos.x = transform.position.x - 1f + transform.childCount * 0.7f;
                break;
        }

        if (textCapacity != null)
        {
            UpdateText();
        }

        return humanPos;
    }

    public void UpdateText()
    {
        if (roomType == RoomType.SwimPool)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 newPos = transform.GetChild(i).position;
                newPos.y = -1.74f;
                newPos.x = transform.position.x - 2f + i * 0.66f;
                transform.GetChild(i).position = newPos;
            }
        }
        if (roomType == RoomType.Nursery)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 newPos = transform.GetChild(i).position;
                newPos.y = -0.12f;
                newPos.x = transform.position.x - 0.61f + i * 1f;
                transform.GetChild(i).position = newPos;
            }
        }
        if (roomType == RoomType.WaitingRoom)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 newPos = transform.GetChild(i).position;
                newPos.x = transform.position.x - 1f + i * 0.7f;
                transform.GetChild(i).position = newPos;
            }
        }

        if (textCapacity == null) return;
        textCapacity.text = $"{transform.childCount}/{maxHumans}";
    }

    public enum RoomType
    {
        SwimPool,
        Nursery,
        WaitingRoom,
        WalkFloor
    }
}

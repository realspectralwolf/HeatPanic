using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct Range
{
    public float min;
    public float max;
}

public class Location : MonoBehaviour
{
    [SerializeField] public RoomType roomType;
    [SerializeField] int maxCapacity = 3;
    [SerializeField] float spawnStart;
    [SerializeField] float spawnEnd;
    [SerializeField] float spawnHeight;
    [SerializeField] bool forceIntoPositions = true;
    [SerializeField] float occupantsSpacing = 2;
    [SerializeField] TextMeshProUGUI textCapacity;
    [SerializeField] HumanState[] acceptableStates;
    [SerializeField] HumanState[] instaKillStates;

    public Range walkRange;
    private List<Human> occupants = new();

    public bool DoesAcceptState(HumanState state)
    {
        for (int i = 0; i < acceptableStates.Length; i++)
        {
            if (acceptableStates[i].GetType() == state.GetType())
            {
                return true;
            }
        }
        return false;
    }

    public bool DoesInstaKillState(HumanState state)
    {
        for (int i = 0; i < instaKillStates.Length; i++)
        {
            if (instaKillStates[i].GetType() == state.GetType())
            {
                return true;
            }
        }
        return false;
    }

    public bool HasFreeSpace()
    {
        if (maxCapacity == -1) return true;

        if (occupants.Count >= maxCapacity)
        {
            // show no space left
            Shake(textCapacity.transform);
            return false;
        }
        return true;
    }

    private void Shake(Transform target)
    {
        Vector3 originalPos = target.position;
        float shakeAmount = 0.1f;

        LeanTween.move(target.gameObject, originalPos + Random.insideUnitSphere * shakeAmount, 0.3f)
            .setEase(LeanTweenType.easeShake)
            .setOnComplete(() => {
                LeanTween.move(target.gameObject, originalPos, 0.5f).setEase(LeanTweenType.easeOutQuad);
            });
    }

    public void UpdateCapacity()
    {
        if (textCapacity == null) return;
        textCapacity.text = $"{occupants.Count}/{maxCapacity}";
    }

    public void RedistributeOccupants()
    {
        for (int i = 0; i < occupants.Count; i++)
        {
            Vector3 pos = occupants[i].transform.position;
            pos.y = spawnHeight;

            if (forceIntoPositions)
                pos.x = spawnStart + i * (spawnEnd - spawnStart) / occupants.Count;

            pos.z = 0;
            occupants[i].transform.position = pos;
        }
    }

    public void AddHuman(Human newHuman)
    {
        occupants.Add(newHuman);
        UpdateCapacity();
        RedistributeOccupants();
    }

    public void RemoveHuman(Human oldHuman)
    {
        occupants.Remove(oldHuman);
        UpdateCapacity();
        RedistributeOccupants();
    }

    public enum RoomType
    {
        SwimPool,
        Nursery,
        WaitingRoom,
        WalkFloor
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float radius = 0.25f;

        Vector3 pos1 = transform.position;
        pos1.y = spawnHeight;
        pos1.x = spawnStart;

        Vector3 pos2 = transform.position;
        pos2.y = spawnHeight;
        pos2.x = spawnEnd;

        Gizmos.DrawSphere(pos1, radius);
        Gizmos.DrawSphere((pos1 + pos2)/2f, radius);
        Gizmos.DrawSphere(pos2, radius);
    }
}

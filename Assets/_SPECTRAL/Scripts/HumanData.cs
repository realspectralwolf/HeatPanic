using UnityEngine;
[CreateAssetMenu(fileName = "HumanData", menuName = "ScriptableObjects/HumanData", order = 1)]
public class HumanData : ScriptableObject
{
    [Header("Human Settings")]
    public float maxHp = 100f;
    public float walkSpeed = 2f;
    public float eventsDelayMin = 5;
    public float eventsDelayMax = 20;
    public float hpfallspeed = 10;
    public float hpregspeed = 10;

    public GameObject poofEffectPrefab;
    public GameObject OnePlusEffectPrefab;

    public HumanState walkState;
    public HumanState fireState;
    public HumanState sleepState;
}
public enum MoveState
{
    Walking,
    Idle,
    Regenerating,
    Waiting,
    Dragged
}

public enum BodyState
{
    Normal,
    OnFire,
    Fainted
}
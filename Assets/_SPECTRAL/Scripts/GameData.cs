using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Global Settings")]
    public int humanDeathsLimit = 10;
    public float tempChangeSpeed = 1.25f;
    public float maxTemp = 150f;
    public float moneyMakingSpeed = 6;
    public float howOftenSpawnHuman = 5;

    [Header("Prefabs")]
    public Human humanPrefab;
    public GameoverUI gameoverUI;
    public GameObject vfx_wrongRoom;
}
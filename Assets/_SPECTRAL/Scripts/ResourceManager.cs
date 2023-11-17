using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int money = 0;
    public int deaths = 0;
    public float temperature = 0;

    public bool gameInProgress = true;
    public bool enableSpawning = true;

    public static System.Action OnGameEnded;
    public static System.Action<int> OnMoneyChanged;
    public static System.Action<int> OnDeathsChanged;

    public static ResourceManager Instance;

    private float timeLeftToSpawnHuman = 0;
    private float startTime;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        startTime = Time.time;
    }

    private void OnEnable()
    {
        Human.OnDied += DecreasePeople;
    }

    void Update()
    {
        temperature += DataMgr.Instance.GameData.tempChangeSpeed * Time.deltaTime;
        temperature = Mathf.Clamp(temperature, 0, DataMgr.Instance.GameData.maxTemp);

        HandleSpawningTick();
    }

    void HandleSpawningTick()
    {
        if (!enableSpawning) return;

        timeLeftToSpawnHuman -= Time.deltaTime;
        if (timeLeftToSpawnHuman <= 0)
        {
            SpawnNewHuman();
            timeLeftToSpawnHuman = DataMgr.Instance.GameData.howOftenSpawnHuman;
        }
    }

    public void AddMoney()
    {
        money++;
        OnMoneyChanged?.Invoke(money);
    }

    public void DecreasePeople()
    {
        deaths++;
        OnDeathsChanged?.Invoke(deaths);

        if (deaths >= DataMgr.Instance.GameData.humanDeathsLimit && gameInProgress)
        {
            gameInProgress = false;
            bool isNewRecord = false;
            int highscore = FileWriter.ReadHighscoreFromFile();

            if (money > highscore)
            {
                FileWriter.WriteHighscoreToFile(money);
                isNewRecord = true;
            }

            Instantiate(DataMgr.Instance.GameData.gameoverUI).Init(money, isNewRecord, highscore);

            OnGameEnded?.Invoke();
            AudioMgr.instance.PlayAudioClip("gameover");
            AudioMgr.instance.StopMusic();
        }
    }

    void SpawnNewHuman()
    {
        var newHuman = Instantiate(DataMgr.Instance.GameData.humanPrefab);
        newHuman.Init();
        DragDropManager.Instance.SendBackToRandomFloor(newHuman);
    }

    public float GetNormalizedTemp()
    {
        return temperature / DataMgr.Instance.GameData.maxTemp;
    }
}

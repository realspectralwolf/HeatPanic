using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int Money = 0;
    public int Deaths = 0;
    public float Temperature = 0;
    public bool IsGameInProgress = true;
    public bool DoEnableSpawning = true;

    public static System.Action<int> OnMoneyChange;
    public static System.Action<int> OnDeathsChange;

    public static ResourceManager Instance;

    private float timeLeftToSpawnHuman = 0;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    }

    private void OnEnable()
    {
        Human.OnDied += DecreasePeople;
    }

    void Update()
    {
        if (!IsGameInProgress) return;

        Temperature += DataHolder.Instance.GameData.tempChangeSpeed * Time.deltaTime;
        Temperature = Mathf.Clamp(Temperature, 0, DataHolder.Instance.GameData.maxTemp);

        HandleSpawningTick();
    }

    void HandleSpawningTick()
    {
        if (!DoEnableSpawning) return;

        timeLeftToSpawnHuman -= Time.deltaTime;
        if (timeLeftToSpawnHuman <= 0)
        {
            SpawnNewHuman();
            timeLeftToSpawnHuman = DataHolder.Instance.GameData.howOftenSpawnHuman;
        }
    }

    public void AddMoney()
    {
        if (!IsGameInProgress) return;

        Money++;
        OnMoneyChange?.Invoke(Money);
    }

    public void DecreasePeople()
    {
        if (!IsGameInProgress) return;

        Deaths++;
        OnDeathsChange?.Invoke(Deaths);

        if (Deaths >= DataHolder.Instance.GameData.humanDeathsLimit)
        {
            IsGameInProgress = false;
            bool isNewRecord = false;
            int highscore = FileWriter.ReadHighscoreFromFile();

            if (Money > highscore)
            {
                FileWriter.WriteHighscoreToFile(Money);
                isNewRecord = true;
            }

            Instantiate(DataHolder.Instance.GameData.gameoverUI).Init(Money, isNewRecord, highscore);
            AudioManager.Instance.PlayAudioClip("gameover");
            AudioManager.Instance.StopMusic();
            gameObject.SetActive(false);
        }
    }

    void SpawnNewHuman()
    {
        var newHuman = Instantiate(DataHolder.Instance.GameData.humanPrefab);
        newHuman.Init();
        DragDropManager.Instance.SendBackToRandomFloor(newHuman);
    }

    public float GetNormalizedTemp()
    {
        return Temperature / DataHolder.Instance.GameData.maxTemp;
    }
}

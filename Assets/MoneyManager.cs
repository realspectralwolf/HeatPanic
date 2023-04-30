using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    public int coins = 0;

    public TextMeshProUGUI deathsText;

    public int people = 0;

    public static MoneyManager instance;

    public int deaths = 0;

    [SerializeField] GameObject humanPrefab;
    [SerializeField] GameObject notEnoughMoneyForHire;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] float spawnHumanFrequency;
    [SerializeField] GameObject canvasGameover;

    [SerializeField] TextMeshProUGUI gameOverHighscoreText;
    [SerializeField] TextMeshProUGUI gameOverTimerText;
    [SerializeField] GameObject gameOverNewHighscore;
    [SerializeField] AudioSource music;
    [SerializeField] CameraShake camShake;

    private float startTime;
    public bool gameInProgress = true;

    int score = 0;

    public static System.Action OnGameEnded;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        startTime = Time.time;  // save the start timeS
        StartCoroutine(HumanSpawningRoutine());
        UpdateCoinsUI();
    }

    IEnumerator HumanSpawningRoutine()
    {
        while (gameInProgress)
        {
            yield return new WaitForSeconds(spawnHumanFrequency);
            SpawnNewHuman();
        }
    }

    void Update()
    {
        if (!gameInProgress) return;

        timerText.text = GetTimeString();
    }

    string GetTimeString()
    {
        float timePassed = Time.time - startTime;  // calculate time passed
        TimeSpan timeSpan = TimeSpan.FromSeconds(timePassed);  // convert to TimeSpan

        // format the time difference as minutes and seconds
        string timeString = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        return timeString;
    }

    private void UpdateCoinsUI()
    {
        scoreText.text = $"Money Earned: {score}$";
    }

    public void AddCoin()
    {
        score++;
        UpdateCoinsUI();
        scoreText.transform.DOKill();
        scoreText.transform.localScale = Vector3.one;
        scoreText.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10);
    }

    public void IncreasePeople()
    {
        people++;
    }

    public void DecreasePeople()
    {
        people--;
        deaths++;
        deathsText.text = $"{deaths}/10";
        deathsText.transform.parent.DOKill();
        deathsText.transform.parent.localScale = Vector3.one;
        deathsText.transform.parent.DOPunchScale(Vector3.one * 1.032f, 0.3f, 5);
        camShake.Shake();

        AudioMgr.instance.PlayAudioClip("death");

        if (deaths >= 10 && gameInProgress)
        {
            Debug.Log("Game Over");
            gameInProgress = false;
            canvasGameover.SetActive(true);
            string currentTimeString = GetTimeString();
            gameOverTimerText.text = $"{score}$";

            int highscore = PlayerPrefs.GetInt("highscoreINT", 0);
            gameOverHighscoreText.text = $"Highscore: {highscore}$";

            if (score > highscore)
            {
                gameOverNewHighscore.SetActive(true);
                PlayerPrefs.SetInt("highscoreINT", score);
            }

            OnGameEnded?.Invoke();
            AudioMgr.instance.PlayAudioClip("gameover");
            music.Stop();
        }
    }

    void SpawnNewHuman()
    {
        var newHuman = Instantiate(humanPrefab).GetComponent<Human>(); ;
        ClickManager.instance.SendBackToRandomFloor(newHuman);
    }

    public void ClickedHirePerosnButton()
    {
        int cost = 10;
        if (coins >= cost)
        {
            SpawnNewHuman();
            coins -= cost;
            moneyText.text = $"{coins}";
        }
        else
        {
            notEnoughMoneyForHire.SetActive(false);
        }
    }
}

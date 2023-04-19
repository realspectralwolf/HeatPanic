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

    [SerializeField] float spawnHumanFrequency;
    [SerializeField] GameObject canvasGameover;

    [SerializeField] TextMeshProUGUI gameOverHighscoreText;
    [SerializeField] TextMeshProUGUI gameOverTimerText;
    [SerializeField] GameObject gameOverNewHighscore;

    private float startTime;
    public bool gameInProgress = true;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startTime = Time.time;  // save the start timeS
        StartCoroutine(HumanSpawningRoutine());
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

    public void AddCoin()
    {
        coins++;
        moneyText.text = $"{coins}";
       // moneyText.transform.DOPunchScale(new Vector3(1, 1.1f, 1), 0.1f);
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
        deathsText.transform.localScale = Vector3.one;
        deathsText.transform.parent.DOPunchScale(Vector3.one * 1.03f, 0.42f);

        if (deaths >= 10)
        {
            Debug.Log("Game Over");
            gameInProgress = false;
            canvasGameover.SetActive(true);
            string currentTimeString = GetTimeString();
            gameOverTimerText.text = currentTimeString; 


            string highscoreString = PlayerPrefs.GetString("highscore", "00:00");
            gameOverHighscoreText.text = $"Highscore {highscoreString}";

            TimeSpan timeSpan = TimeSpan.Parse(highscoreString);
            int secondsHighscore = (int)timeSpan.TotalSeconds;
            int secondsCurrent = (int)TimeSpan.Parse(currentTimeString).TotalSeconds;

            if (secondsCurrent > secondsHighscore)
            {
                gameOverNewHighscore.SetActive(true);
                PlayerPrefs.SetString("highscore", currentTimeString);
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int highscore = PlayerPrefs.GetInt("highscoreINT", 0);
        GetComponent<TextMeshProUGUI>().text = $"Highscore: {highscore}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

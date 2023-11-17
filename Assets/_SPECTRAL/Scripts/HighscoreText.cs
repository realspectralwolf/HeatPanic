using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int highscore = FileWriter.ReadHighscoreFromFile();
        GetComponent<TextMeshProUGUI>().text = $"Highscore: {highscore}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

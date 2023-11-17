using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameoverUI : MonoBehaviour
{
    List<Vector3> initialScales = new();
    [SerializeField] Transform[] objects;
    [SerializeField] Transform mainButton;
    [SerializeField] float animSpeed;
    [SerializeField] float delayBetweenObjs;
    [SerializeField] Transform background;
    [SerializeField] float bgAnimSpeed;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] GameObject newHighscoreUI;

    public void Init(int money, bool isNewRecord, int highscore)
    {
        ScaleAllObjectsToZero();

        background.localPosition = new Vector3(0, -1080, 0);
        background.LeanMoveLocal(Vector3.zero, bgAnimSpeed).setOnComplete(() => { StartCoroutine(AnimateScaleOfObjects()); });

        scoreText.text = $"{money}$";
        highscoreText.text = $"Highscore: {highscore}$";
        newHighscoreUI.SetActive(isNewRecord);
    }

    void ScaleAllObjectsToZero()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];
            initialScales.Add(obj.localScale);
            obj.localScale = Vector3.zero;
        }
    }

    IEnumerator AnimateScaleOfObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];
            obj.LeanScale(initialScales[i], animSpeed);
            yield return new WaitForSeconds(delayBetweenObjs);
            if (mainButton == obj)
            {
                mainButton.LeanScale(Vector3.one * 1.08f, 0.86f).setLoopCount(-1).setLoopPingPong();
            }
        }
    }

    public void OnClickAgainButton()
    {
        SceneMgr.instance.LoadGameplayScene();
    }

    public void OnClickMenuButton()
    {
        SceneMgr.instance.LoadMenuScene();
    }
}

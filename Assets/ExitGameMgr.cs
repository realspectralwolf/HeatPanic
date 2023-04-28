using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExitGameMgr : MonoBehaviour
{
    [SerializeField] float timeToHold;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text_timeLeft;

    float currentTimeHeldDown = 0;
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            currentTimeHeldDown = 0;
            canvas.SetActive(true);
        }
        if (Input.GetButtonUp("Cancel"))
        {
            canvas.SetActive(false);
        }

        if (Input.GetButton("Cancel"))
        {
            currentTimeHeldDown = Mathf.Clamp(currentTimeHeldDown + Time.deltaTime, 0, timeToHold);
            slider.value = currentTimeHeldDown / timeToHold;
            text_timeLeft.text = (timeToHold - currentTimeHeldDown).ToString("0.00") + "s";
            if (currentTimeHeldDown == timeToHold)
            {
                SceneMgr.instance.LoadMenuScene();
            }
        }
    }
}

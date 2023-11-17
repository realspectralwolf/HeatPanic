using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureBar : MonoBehaviour
{
    Slider ui_slider;

    private void Start()
    {
        ui_slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        ui_slider.value = ResourceManager.Instance.GetNormalizedTemp();
    }
}

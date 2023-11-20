using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ResourceManager.OnMoneyChange += UpdateValue;
    }

    private void OnDisable()
    {
        ResourceManager.OnMoneyChange -= UpdateValue;
    }

    private void UpdateValue(int newValue)
    {
        LeanTween.cancel(transform.gameObject);
        transform.localScale = Vector3.one;
        transform.LeanScale(Vector3.one * 0.7f, 0.6f).setEasePunch();
        text.text = $"Money Earned: {newValue}$";
    }
}

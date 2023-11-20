using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathsText : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ResourceManager.OnDeathsChange += UpdateValue;
    }

    private void OnDisable()
    {
        ResourceManager.OnDeathsChange -= UpdateValue;
    }

    private void UpdateValue(int newValue)
    {
        LeanTween.cancel(transform.gameObject);
        transform.localScale = Vector3.one;
        transform.LeanScale(Vector3.one * 0.1f, 0.3f).setEasePunch();
        text.text = $"{newValue}/{DataHolder.Instance.GameData.humanDeathsLimit}";
    }
}

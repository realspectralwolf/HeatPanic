using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TutorialComplete : MonoBehaviour
{
    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.LeanScale(Vector3.one * 1, 0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialHintAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(Vector3.one * 1.08f, 0.36f).SetLoops(-1, LoopType.Yoyo);
    }
}

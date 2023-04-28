using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] Transform playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.DOScale(Vector3.one * 1.08f, 0.36f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

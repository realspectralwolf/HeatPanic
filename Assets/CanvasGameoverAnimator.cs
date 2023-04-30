using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasGameoverAnimator : MonoBehaviour
{
    List<Vector3> initialScales = new();
    [SerializeField] Transform[] objects;
    [SerializeField] Transform mainButton;
    [SerializeField] float animSpeed;
    [SerializeField] float delayBetweenObjs;
    [SerializeField] Transform background;
    [SerializeField] float bgAnimSpeed;

    void OnEnable()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];
            initialScales.Add(obj.localScale);
            obj.localScale = Vector3.zero;
        }

        background.localPosition = new Vector3(0, -1080, 0);
        background.DOLocalMove(Vector3.zero, bgAnimSpeed).OnComplete(() => { StartCoroutine(AnimateObjects());   });
    }

    IEnumerator AnimateObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];
            obj.DOScale(initialScales[i], animSpeed);
            yield return new WaitForSeconds(delayBetweenObjs);

            if (mainButton == obj)
            {
                mainButton.DOScale(Vector3.one * 1.08f, 0.36f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}

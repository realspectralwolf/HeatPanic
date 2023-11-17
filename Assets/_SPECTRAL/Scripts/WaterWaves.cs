using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WaterWaves : MonoBehaviour
{
    [SerializeField] float targetY = 0.3f;
    [SerializeField] float animSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.LeanMoveLocalY(targetY, animSpeed).setLoopCount(-1).setLoopPingPong().setEase(LeanTweenType.easeInOutCubic);
    }
}

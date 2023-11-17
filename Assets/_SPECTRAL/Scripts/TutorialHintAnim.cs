using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHintAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.LeanScale(Vector3.one * 1.08f, 0.36f).setLoopCount(-1).setLoopPingPong();
    }
}

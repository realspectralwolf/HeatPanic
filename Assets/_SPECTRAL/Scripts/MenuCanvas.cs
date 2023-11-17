using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuCanvas : MonoBehaviour
{
    [SerializeField] Transform playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.LeanScale(Vector3.one * 1.08f, 0.36f).setLoopCount(-1).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

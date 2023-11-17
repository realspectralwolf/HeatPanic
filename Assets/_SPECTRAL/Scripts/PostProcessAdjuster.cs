using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessAdjuster : MonoBehaviour
{
    private Bloom bloom;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Volume>().profile.TryGet(out bloom);
    }

    // Update is called once per frame
    void Update()
    {
        bloom.intensity.Override(Mathf.Lerp(0.35f, 2f, ResourceManager.Instance.GetNormalizedTemp()));
    }
}

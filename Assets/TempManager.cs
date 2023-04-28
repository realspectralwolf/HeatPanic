using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class TempManager : MonoBehaviour
{
    [SerializeField] Color camColor1;
    [SerializeField] Color camColor2;

    [SerializeField] public float temp = 0;
    [SerializeField] float tempChangeSpeed = 0.5f;
    [SerializeField] public float maxTemp = 100;

    [SerializeField] Slider tempSlider;
    [SerializeField] Volume volume;

    [SerializeField] Transform sunTransform;
    [SerializeField] AudioSource musicSource;

    private Bloom bloom;

    public static TempManager instance;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out bloom);
    }

    public float a = 0;
    // Update is called once per frame
    void Update()
    {
        temp += tempChangeSpeed * Time.deltaTime;
        a = temp / maxTemp;
        tempSlider.value = a;
        Camera.main.backgroundColor = Color.Lerp(camColor1, camColor2, a);
        bloom.intensity.Override(Mathf.Lerp(0.35f, 2f, a));
        sunTransform.localScale = Vector3.Lerp(Vector3.one * 0.44f, Vector3.one * 1.2f, a);
        musicSource.pitch = Mathf.Lerp(1, 1f, a);
    }
}

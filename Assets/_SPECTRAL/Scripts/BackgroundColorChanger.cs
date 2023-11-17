using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField] Color camColor1;
    [SerializeField] Color camColor2;

    // Update is called once per frame
    void Update()
    {
        Camera.main.backgroundColor = Color.Lerp(camColor1, camColor2, ResourceManager.Instance.GetNormalizedTemp());

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAnimator : MonoBehaviour
{
    [SerializeField] float rotSpeed = 1;
    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(Vector3.one * 0.44f, Vector3.one * 1.2f, ResourceManager.Instance.GetNormalizedTemp());
        transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
    }
}

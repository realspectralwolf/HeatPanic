using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.3f;
	[SerializeField] private float shakeAmplitude = 0.23f;
    [SerializeField] private float decreaseFactor = 1.5f;

    private Transform camTransform;
    private float shakeTimer = 0;
    private Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;

		Human.OnDied += Shake;
    }

    void OnDisable()
    {
        Human.OnDied -= Shake;
    }

    void Update()
	{
		if (shakeTimer > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmplitude;

			shakeTimer -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeTimer = 0f;
			camTransform.localPosition = originalPos;
		}
	}

	public void Shake()
    {
		shakeTimer = shakeDuration;
	}
}
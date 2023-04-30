using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0.3f;
	private float shakeTimer = 0;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	public bool testButton;

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
	}

	void Update()
	{
		if (shakeTimer > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeTimer -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeTimer = 0f;
			camTransform.localPosition = originalPos;
		}

		if (testButton)
        {
			testButton = false;
			Shake();
		}
	}

	public void Shake()
    {
		shakeTimer = shakeDuration;
	}
}
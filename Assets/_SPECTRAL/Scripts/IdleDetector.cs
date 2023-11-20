using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleDetector : MonoBehaviour
{
    [SerializeField] public float timeSinceLastInput = 0f;
    [SerializeField] public float idleSeconds = 60f;

    void Update()
    {
        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            // Reset the timer if there is any input
            timeSinceLastInput = 0f;
        }

        // Increment the timer
        timeSinceLastInput += Time.deltaTime;

        // Check if the timer exceeds 1 minute (60 seconds)
        if (timeSinceLastInput >= idleSeconds)
        {
            // Player has had no input for 1 minute, perform desired actions here
            SceneChanger.Instance.LoadMenuScene();
        }
    }
}

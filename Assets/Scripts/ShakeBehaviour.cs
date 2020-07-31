using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehaviour : MonoBehaviour
{

    #region Static
    public static ShakeBehaviour instance;

    private void Awake()
    {
        instance = this;

        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }

    }

    #endregion


    // Transform of the GameObject you want to shake
    private Transform transform;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 5f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }


    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        shakeDuration = 0.2f;
    }

}

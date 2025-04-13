using UnityEngine;

public class ShakyCamera : MonoBehaviour
{
    public float shakeDuration = 0.1f;  // Duration of the shake
    public float shakeIntensity = 0.1f;  // Intensity of the shake

    private Vector3 originalPosition;
    private Vector3 shakeOffset; // Offset for shaking

    void Start()
    {
        originalPosition = transform.localPosition; // Save the initial local position
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeIntensity;  // Random shake offset
            transform.localPosition = originalPosition + shakeOffset;  // Apply shake to local position
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;  // Reset to original position once shake ends
        }
    }

    // Call this function to start shaking
    public void Shake(float duration, float intensity)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
    }
}
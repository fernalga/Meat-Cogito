using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    private Light lightToFlicker;
    [SerializeField, Range(0f, 1000f)] private float minIntensity = 0.5f;
    [SerializeField, Range(0f, 1000f)] private float maxIntensity = 0.5f;
    [SerializeField, Min(0f)] private float timeBetweenIntensity = 0.1f;
    
    public Light childLight;
    public Light otherLight;
    public Light otherLightChild;
    
    public bool enableProximityAutoOff = false;
    public float autoOffDelay = 10f;
    private bool isCountingDown = false;

    private float currentTimer;
    
    [SerializeField] private AudioSource flickerAudio;
    private void Awake()
    {
        if (lightToFlicker == null)
        {
            lightToFlicker = GetComponent<Light>();
        }

        ValidateIntensityBounds();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.name}");
        if (!enableProximityAutoOff) return;

        if (other.CompareTag("Player") && !isCountingDown)
        {
            StartCoroutine(ShutOffAfterDelay());
        }
    }
    
    private IEnumerator ShutOffAfterDelay()
    {
        isCountingDown = true;
        yield return new WaitForSeconds(autoOffDelay);
        if (lightToFlicker != null)
        {
            lightToFlicker.enabled = false;
        }
        
        if (flickerAudio != null)
        {
            flickerAudio.Stop();         // Stops the sound
        }
        
        // Turn on the other light when the first light turns off
        if (otherLight != null)
        {
            otherLight.enabled = true;
        }
        
        if (childLight != null)
        {
            childLight.enabled = false;
        }
        
        if (otherLightChild != null)
        {
            otherLightChild.enabled = true;
        }
    }
    
    private void Update()
    {
        currentTimer += Time.deltaTime;
        if (!(currentTimer >= timeBetweenIntensity)) return;
        lightToFlicker.intensity = Random.Range(minIntensity, maxIntensity);
        currentTimer = 0;
    }

    private void ValidateIntensityBounds()
    {
        if (!(minIntensity > maxIntensity))
        {
            return;
        }
        Debug.LogWarning("Min Intensity is greater than max Intensity, Swapping values!");
        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
}

using UnityEngine;

public class FlickerAlarmManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PlayerTriggerCheck triggerZone;
    [SerializeField] private GameObject alarmParent; // The parent GameObject holding all FlickeringLight objects
    [SerializeField] private Color alarmColor = Color.red;

    private bool alarmActivated = false;

    void Awake()
    {
        DisableAllFlickers();
    }

    void Update()
    {
        if (!alarmActivated && triggerZone.hasPlayerEntered)
        {
            alarmActivated = true;
            EnableAllFlickersAndColor();
            EnableAllAudioSources();
        }
    }


    private void DisableAllFlickers()
    {
        if (alarmParent == null) return;

        FlickeringLight[] flickers = alarmParent.GetComponentsInChildren<FlickeringLight>(true);
        foreach (var flicker in flickers)
        {
            flicker.enabled = false;
        }
    }
    
    private void EnableAllAudioSources()
    {
        if (alarmParent == null) return;

        AudioSource[] audioSources = alarmParent.GetComponentsInChildren<AudioSource>(true);
        foreach (var audio in audioSources)
        {
            audio.Play();
        }
    }


    private void EnableAllFlickersAndColor()
    {
        if (alarmParent == null) return;

        FlickeringLight[] flickers = alarmParent.GetComponentsInChildren<FlickeringLight>(true);
        foreach (var flicker in flickers)
        {
            // Enable the flickering script
            flicker.enabled = true;

            // Change light color to red
            Light lightComponent = flicker.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.color = alarmColor;
            }
        }
    }
}
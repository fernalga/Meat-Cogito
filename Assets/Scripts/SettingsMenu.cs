using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixers;
    public void SetVolume(float volume)
    {
        audioMixers.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
}

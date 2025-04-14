using System.Collections;
using UnityEngine;
using TMPro; // <- TextMeshPro namespace
using UnityEngine.SceneManagement;

public class DelayLevel : MonoBehaviour
{
    public TMP_Text crashText;
    public ShakyCamera firstPersonCam;
    public ShakyCamera thirdPersonCam;
    
    [SerializeField] private float delayTime = 10f;
    [SerializeField] private string sceneToLoad = "Level2";


    bool loadingStarted = false;
    float secondsLeft = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !loadingStarted)
        {
            if (crashText != null)
                crashText.gameObject.SetActive(true);

            if (firstPersonCam != null)
                firstPersonCam.Shake(10f, 0.02f);

            if (thirdPersonCam != null)
                thirdPersonCam.Shake(10f, 0.02f);

            StartCoroutine(DelayLoadLevel(delayTime));
        }
    }

    IEnumerator DelayLoadLevel(float seconds)
    {
        secondsLeft = seconds;
        loadingStarted = true;

        while (secondsLeft > 0)
        {
            if (crashText != null)
                crashText.text = "Reactor Meltdown in\n" + Mathf.CeilToInt(secondsLeft);

            yield return new WaitForSeconds(1);
            secondsLeft--;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
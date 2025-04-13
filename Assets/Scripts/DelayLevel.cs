using System.Collections;
using UnityEngine;
using TMPro; // <- TextMeshPro namespace
using UnityEngine.SceneManagement;

public class DelayLevel : MonoBehaviour
{
    public TMP_Text crashText;
    public ShakyCamera firstPersonCam;
    public ShakyCamera thirdPersonCam;


    bool loadingStarted = false;
    float secondsLeft = 0;

    void OnTriggerEnter(Collider other)
    {
        if (!loadingStarted)
        {
            crashText.gameObject.SetActive(true);
            firstPersonCam.Shake(10f, 0.02f);
            thirdPersonCam.Shake(10f, 0.02f);
            StartCoroutine(DelayLoadLevel(10));
        }
    }

    IEnumerator DelayLoadLevel(float seconds)
    {
        secondsLeft = seconds;
        loadingStarted = true;

        while (secondsLeft > 0)
        {
            crashText.text = "Reactor Meltdown in\n" + Mathf.CeilToInt(secondsLeft);
            yield return new WaitForSeconds(1);
            secondsLeft--;
        }

        SceneManager.LoadScene("Level2");
    }
}
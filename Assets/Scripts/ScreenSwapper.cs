using UnityEngine;

public class ScreenSwapper : MonoBehaviour
{
    public GameObject currentObjectToHide;
    public GameObject nextObjectToShow;

    public void SwapScreens()
    {
        if (currentObjectToHide != null)
            currentObjectToHide.SetActive(false);

        if (nextObjectToShow != null)
            nextObjectToShow.SetActive(true);
    }
}


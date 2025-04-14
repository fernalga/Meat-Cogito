using UnityEngine;

public class PlayerTriggerCheck : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject objectToActivate;

    public bool hasPlayerEntered { get; private set; } = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            hasPlayerEntered = true;

            if (objectToActivate != null)
                objectToActivate.SetActive(true);
        }
    }
}
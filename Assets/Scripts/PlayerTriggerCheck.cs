using UnityEngine;

public class PlayerTriggerCheck : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;

    public bool hasPlayerEntered { get; private set; } = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            hasPlayerEntered = true;

            if (objectToActivate != null)
                objectToActivate.SetActive(true);
            
            if(objectToDeactivate != null)
                objectToDeactivate.SetActive(false);
        }
    }
}
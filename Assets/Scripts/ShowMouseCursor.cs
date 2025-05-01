using UnityEngine;

public class ShowMouseCursor : MonoBehaviour
{
    void Start()
    {
        // Make the cursor visible
        Cursor.visible = true;
        
        // Unlock the cursor (allows free movement)
        Cursor.lockState = CursorLockMode.None;
    }
}
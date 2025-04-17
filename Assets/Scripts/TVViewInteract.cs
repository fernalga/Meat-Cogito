using UnityEngine;

public class TVViewInteract : MonoBehaviour
{
    public Transform tvCamAnchor;
    private Vector3 originalCamPosition;
    private Quaternion originalCamRotation;
    public Camera pickUpCamera;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    public bool isViewing = false;
    
    public CharacterController playerController;

    public void ViewTV()
    {
        PlayerCamera.CameraBlocker.BlockCameraInput = true;
        playerController.canMove = false;
        
        CursorManager.Instance.RequestCursor("TVView");
        
        // Save pickUpCamera positon at the moment of interaction
        originalCamPosition = pickUpCamera.transform.position;
        originalCamRotation = pickUpCamera.transform.rotation;
        
        // Snap to TV view
        pickUpCamera.transform.position = tvCamAnchor.position;
        pickUpCamera.transform.rotation = tvCamAnchor.rotation;

        pickUpCamera.enabled = true;

        isViewing = true;
    }

    public void ExitView()
    {
        PlayerCamera.CameraBlocker.BlockCameraInput = false;
        playerController.canMove = true;

        CursorManager.Instance.ReleaseCursor("TVView");
            
        pickUpCamera.transform.position = originalCamPosition;
        pickUpCamera.transform.rotation = originalCamRotation;
        
        pickUpCamera.enabled = false;
        firstPersonCamera.gameObject.SetActive(true);
        
        isViewing = false;
    }
}
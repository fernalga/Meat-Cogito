using UnityEngine;

public class TVViewInteract : MonoBehaviour
{
    public Transform tvCamAnchor;
    private Vector3 originalCamPosition;
    private Transform originalParent;
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
        originalCamPosition = firstPersonCamera.transform.position;
        originalCamRotation = firstPersonCamera.transform.rotation;
        originalParent = firstPersonCamera.transform.parent;
        
        // Snap to TV view
        firstPersonCamera.transform.SetParent(tvCamAnchor); // Parent to anchor
        firstPersonCamera.transform.localPosition = Vector3.zero;
        firstPersonCamera.transform.localRotation = Quaternion.identity;

        pickUpCamera.enabled = true;

        isViewing = true;
    }

    public void ExitView()
    {
        PlayerCamera.CameraBlocker.BlockCameraInput = false;
        playerController.canMove = true;

        CursorManager.Instance.ReleaseCursor("TVView");
        firstPersonCamera.transform.SetParent(originalParent);
        firstPersonCamera.transform.position = originalCamPosition;
        firstPersonCamera.transform.rotation = originalCamRotation;
        
        pickUpCamera.enabled = false;
        firstPersonCamera.gameObject.SetActive(true);
        
        isViewing = false;
    }
}
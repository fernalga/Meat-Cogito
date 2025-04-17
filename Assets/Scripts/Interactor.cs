using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform interactionPoint;
    public float interactRange = 5f;

    [Header("Object Handling")]
    public Transform holdPoint;
    public float throwForce = 500f;
    public float rotationSpeed = 5f;
    
    [HideInInspector] public bool isRotatingObject;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private CapsuleCollider playerCollider;
    private TVViewInteract currentTVView;
    
    Rigidbody heldObjectRigidbody;  // Reference to the Rigidbody
    bool isHoldingObject = false;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (PauseMenu.isPaused) return;
        interactionPoint.rotation = GetActiveCamera().transform.rotation;
        Debug.DrawRay(interactionPoint.position, interactionPoint.forward * interactRange, Color.red);

        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject)
                DropObject();
            else
                TryInteract();
        }

        if (heldObject)
        {
            if (Input.GetMouseButtonDown(0)) // Left click to throw
                ThrowObject();

            MoveObject();
            RotateObject();
        }
    }

    void TryInteract()
    {
        if (currentTVView != null && currentTVView.isViewing)
        {
            currentTVView.ExitView();
            currentTVView = null;
            return;
        }
        
        if (!Physics.Raycast(interactionPoint.position, interactionPoint.forward, out RaycastHit hit, interactRange)) return;

        string tag = hit.transform.tag;
        if (tag == "canPickUp")
        {
            PickUpObject(hit.collider.gameObject);
        }
        else if (tag == "TV" && hit.collider.TryGetComponent(out TVViewInteract tv))
        {
            tv.ViewTV();
            currentTVView = tv;
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldObjectRb = obj.GetComponent<Rigidbody>();

        heldObjectRb.isKinematic = true;
        heldObject.transform.SetParent(holdPoint);
        heldObject.layer = LayerMask.NameToLayer("Held");

        playerCollider.enabled = false;
    }

    void DropObject()
    {
        if (!heldObject) return;

        FinalizeObjectRelease();

        heldObject = null;
        heldObjectRb = null;
    }

    void ThrowObject()
    {
        if (!heldObject) return;

        FinalizeObjectRelease();
        heldObjectRb.AddForce(GetActiveCamera().transform.forward * throwForce);

        heldObject = null;
        heldObjectRb = null;
    }

    void FinalizeObjectRelease()
    {
        playerCollider.enabled = true;

        heldObject.layer = 0;
        heldObject.transform.parent = null;

        heldObjectRb.isKinematic = false;
        heldObjectRb.linearVelocity = Vector3.zero;
        heldObjectRb.angularVelocity = Vector3.zero;
    }

    void MoveObject()
    {
        heldObject.transform.position = holdPoint.position;
    }

    void RotateObject()
    {
        if (!Input.GetKey(KeyCode.R))
        {
            isRotatingObject = false;
            return;
        }

        isRotatingObject = true;

        float rotateX = Input.GetAxis("Mouse X") * rotationSpeed;
        float rotateY = Input.GetAxis("Mouse Y") * rotationSpeed;

        heldObject.transform.Rotate(Vector3.up, -rotateX, Space.World);
        heldObject.transform.Rotate(Vector3.right, rotateY, Space.World);
    }

    Camera GetActiveCamera()
    {
        PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();
        return playerCamera.IsFirstPerson() ? playerCamera.FirstPersonCamera : playerCamera.ThirdPersonCamera;
    }
}
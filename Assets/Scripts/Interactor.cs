using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform interactionPoint; // The point from which interaction occurs
    public float interactRange = 5f;   // Range to interact with objects
    public Transform holdPoint;        // Where the object will be held
    public float throwForce = 500f;    // Force to throw the object
    public float rotationSpeed = 5f;   // Speed of rotation
    public bool isRotatingObject = false; // Tracks if rotating object
    
    private GameObject heldObject;     // Currently held object
    private Rigidbody heldObjectRb;    // Rigidbody of the held object

    void Update()
    {
        Debug.DrawRay(interactionPoint.position, interactionPoint.forward * interactRange, Color.red);

        if (!heldObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickUp();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropObject();
            }
            if (Input.GetMouseButtonDown(0)) // Left click to throw object
            {
                ThrowObject();
            }

            MoveObject();
            RotateObject();
        }
    }

    void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(interactionPoint.position, interactionPoint.forward, out hit, interactRange))
        {
            if (hit.transform.CompareTag("canPickUp"))
            {
                PickUpObject(hit.transform.gameObject);
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldObjectRb = obj.GetComponent<Rigidbody>();
        heldObjectRb.isKinematic = true; // Disable physics while holding
        heldObject.transform.parent = holdPoint; // Parent it to the holdPoint
        heldObject.layer = LayerMask.NameToLayer("Held"); // Set layer to Held
        Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>(), true); // Ignore collision with player
    }

    void DropObject()
    {
        if (heldObject)
        {
            // Ensure collision is re-enabled
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>(), false);

            heldObject.layer = 0; // Reset the layer
            heldObject.transform.parent = null; // Fully detach the object

            heldObjectRb.isKinematic = false; // Enable physics
            heldObjectRb.linearVelocity = Vector3.zero; // Reset movement
            heldObjectRb.angularVelocity = Vector3.zero; // Reset spin

            heldObject = null; // Clear the held object reference
        }
    }

    void ThrowObject()
    {
        if (heldObject)
        {
            // Ensure collision is re-enabled
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>(), false);

            heldObject.layer = 0; // Reset the layer
            heldObject.transform.parent = null; // Fully detach the object

            heldObjectRb.isKinematic = false; // Enable physics
            heldObjectRb.linearVelocity = Vector3.zero; // Reset movement
            heldObjectRb.angularVelocity = Vector3.zero; // Reset spin
            heldObjectRb.AddForce(interactionPoint.forward * throwForce);
            heldObject = null; // Clear the held object reference
        }
    }

    void MoveObject()
    {
        if (heldObject)
        {
            heldObject.transform.position = holdPoint.position; // Keep it at the holdPoint
        }
    }
    
    void RotateObject()
    {
        if (heldObject && Input.GetKey(KeyCode.R))
        {
            isRotatingObject = true;
            
            float rotateX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotateY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Rotate the held object based on mouse movement
            heldObject.transform.Rotate(Vector3.up, -rotateX, Space.World);
            heldObject.transform.Rotate(Vector3.right, rotateY, Space.World);
        }
        else isRotatingObject = false;
    }
}

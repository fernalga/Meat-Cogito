using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform interactionPoint; // The point from which interaction occurs
    public float interactRange = 5f;   // Range to interact with objects
    public Transform holdPoint;        // Where the object will be held
    public float throwForce = 500f;    // Force to throw the object

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
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>(), false); // Enable collision
            heldObject.layer = 0; // Reset the layer
            heldObjectRb.isKinematic = false; // Enable physics again
            heldObject.transform.parent = null; // Unparent it
            heldObject = null; // Clear the held object
        }
    }

    void ThrowObject()
    {
        if (heldObject)
        {
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>(), false); // Enable collision
            heldObject.layer = 0; // Reset the layer
            heldObjectRb.isKinematic = false; // Enable physics again
            heldObject.transform.parent = null; // Unparent it
            heldObjectRb.AddForce(interactionPoint.forward * throwForce); // Apply force to throw
            heldObject = null; // Clear the held object
        }
    }

    void MoveObject()
    {
        if (heldObject)
        {
            heldObject.transform.position = holdPoint.position; // Keep it at the holdPoint
        }
    }
}

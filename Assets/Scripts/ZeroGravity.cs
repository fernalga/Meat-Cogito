using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    public float floatStrength = 5f; // How strongly objects float
    public float rotationSpeed = 2f; // How fast objects rotate
    public string[] objectTags = {"FloatingObject" }; // Tags of objects to affect

    private List<Rigidbody> floatingObjects = new List<Rigidbody>();

    void Start()
    {
        // Find all objects with the specified tags and get their Rigidbody components
        foreach (string tag in objectTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    floatingObjects.Add(rb);
                    rb.useGravity = false; // Disable gravity
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Apply floating force and rotation to each object
        foreach (Rigidbody rb in floatingObjects)
        {
            if (rb != null)
            {
                // Apply upward force
                rb.AddForce(Vector3.up * floatStrength);
                
                // Apply random rotation
                rb.AddTorque(Random.insideUnitSphere * rotationSpeed);
            }
        }
    }
    
    public void AddFloatingObject(GameObject newObject)
    {
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        if (rb != null && !floatingObjects.Contains(rb))
        {
            floatingObjects.Add(rb);
            rb.useGravity = false;
        }
    }
}
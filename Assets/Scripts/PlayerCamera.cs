using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] // variable are editable in unity inspector but remain private
    private float defaultDistance = 6f, // sets distance of camera from character
        minDistance = 3f, // determines camera zoom in/out from character 
        maxDistance = 10f,
        distanceMovementSpeed = 5f, // speed of camera zoom in/out
        distanceMovementSharpness = 10f, 
        rotationSpeed = 10f, // determines how fast we rotate camera on character 
        rotationSharpness = 10000f,
        followSharpness = 10000f, 
        minVerticalAngle = -90f, // sets how far camera can look up/down
        maxVerticalAngle = 90f,
        defaultVerticalAngle = 20f; // sets camera angle on start up
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

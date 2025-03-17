using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] // variables are editable in unity inspector but remain private
    // Camera distance settings
    private float _defaultDistance = 0f,
        _minDistance = 0f,
        _maxDistance = 5f,
        _distanceSpeed = 5f,
        _distanceSharpness = 10f,

        // Rotation settings
        _rotationSpeed = 5f,
        _rotationSharpness = 10000f,

        // Follow smoothing
        _followSharpness = 50f,

        // Vertical angle limits (camera up/down)
        _minVerticalAngle = -90f,
        _maxVerticalAngle = 90f;
    
    // Transform references 
    private Transform _followTransform;
    private Vector3 _currentFollowPos, _planarDirection;
    [SerializeField] private Transform _playerTransform;
    
    // Distance and angle tracking
    private float _targetVerticalAngle;
    private float _currentDistance, _targetDistance;
    
    // Camera
    [SerializeField] private Camera _pickupCamera;
    [SerializeField] private Camera _playerCamera;
    
    
    // resets character camera angle/distance/direction back to default on start up
    private void Awake()
    {
        _currentDistance = _defaultDistance;
        _targetDistance = _currentDistance;
        _targetVerticalAngle = 0f;
        _planarDirection = Vector3.forward;
    }

    // Bridges camera,input, and character controller
    public void SetFollowTransform(Transform target)
    {
        _followTransform = target;
        _currentFollowPos = target.position;
        _planarDirection = target.forward;
    }

    // clamps distance and angle to range set by pre-defined parameters 
    private void OnValidate()
    {
        _defaultDistance = Mathf.Clamp(_defaultDistance, _minDistance, _maxDistance);
    }
    
    public bool IsFirstPerson()
    {
        return _currentDistance < 0.1f; // Return if the camera is in first-person mode
    }

    // processes rotation input
    private void HandleRotation(float deltaTime, Vector3 rotationInput, out Quaternion targetRotation)
    {
        bool isFirstPerson = IsFirstPerson(); // Check if in first-person mode

        if (FindObjectOfType<Interactor>().isRotatingObject)
        {
            targetRotation = transform.rotation; // Do nothing if rotating an object
            return;
        }

        // In third-person, apply horizontal rotation based on movement input
        _planarDirection = Quaternion.Euler(0, rotationInput.x * _rotationSpeed, 0) * _planarDirection;
        Quaternion planarRot = Quaternion.LookRotation(_planarDirection, Vector3.up);
        
        // Handle Horizontal Rotation (Yaw)
        if (isFirstPerson)
        {
            // In first-person, rotate horizontally with the mouse input
            _playerTransform.rotation = planarRot;
        }

        // Pitch (Vertical Rotation) logic remains the same
        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle - (rotationInput.y * _rotationSpeed), _minVerticalAngle, _maxVerticalAngle);
        Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);

        // Final Camera Rotation (combined with the vertical angle)
        targetRotation = planarRot * verticalRot;
        transform.rotation = targetRotation; // Apply the final rotation to the camera
    }




    private void HandlePosition(float deltaTime, float zoomInput, Quaternion targetRotation)
    {
        // Adjust Camera Distance
        _targetDistance = Mathf.Clamp(_targetDistance + zoomInput * _distanceSpeed, _minDistance, _maxDistance);

        // Smoothly Follow Target
        _currentFollowPos = Vector3.Lerp(_currentFollowPos, _followTransform.position, 1f - Mathf.Exp(-_followSharpness * deltaTime));

        // Desired Position
        Vector3 desiredPosition = _currentFollowPos - (targetRotation * Vector3.forward * _targetDistance);

        // Raycast to Avoid Clipping
        if (Physics.Raycast(_currentFollowPos, desiredPosition - _currentFollowPos, out RaycastHit hit,
                _targetDistance))
        {
            _currentDistance = Mathf.Lerp(_currentDistance,
                Mathf.Clamp(hit.distance * 0.9f, _minDistance, _maxDistance),
                1 - Mathf.Exp(-_distanceSharpness * deltaTime));
        }
        else
            _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, 1 - Mathf.Exp(-_distanceSharpness * deltaTime));

        // Apply Position
        transform.position = _currentFollowPos - (targetRotation * Vector3.forward * _currentDistance);
    }
    
    private void HandlePickUpCamera()
    {
        bool isFirstPerson = isFirstPerson = IsFirstPerson();
        _pickupCamera.enabled = isFirstPerson;

        if (isFirstPerson)
        {
            // Remove the "Held" layer from PlayerCamera's culling mask
            _playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Held"));
        }
        else
        {
            // Add the "Held" layer to PlayerCamera's culling mask in third-person
            _playerCamera.cullingMask |= (1 << LayerMask.NameToLayer("Held"));
        }
    }

    public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
    {
        if (_followTransform)
        {
            Debug.DrawRay(_playerTransform.position, _playerTransform.forward * 5f, Color.blue);
            HandleRotation(deltaTime, rotationInput, out Quaternion targetRotation);
            HandlePosition(deltaTime, zoomInput, targetRotation);
            HandlePickUpCamera();
        }
    }
}

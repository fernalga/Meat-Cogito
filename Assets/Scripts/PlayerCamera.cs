using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] // variables are editable in unity inspector but remain private
    // Camera distance settings
    private float _defaultDistance = 0f,
        _minDistance = 0f,
        _maxDistance = 3f,
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
    
    private string playerLayerName = "Player";
    private int _playerLayer;
    private LayerMask _originalFirstPersonMask;
    
    // Camera
    private Camera _currentActiveCamera;
    public Camera FirstPersonCamera => _firstPersonCamera;
    public Camera ThirdPersonCamera => _thirdPersonCamera;
    
    public float mouseSensitivity = 1.0f;
    
    [Header("Camera Mode Settings")]
    [SerializeField] private Camera _pickupCamera;
    [SerializeField] private Camera _firstPersonCamera;
    [SerializeField] private Camera _thirdPersonCamera;
    
    
    // resets character camera angle/distance/direction back to default on start up
    private void Awake()
    {
        _playerLayer = LayerMask.NameToLayer(playerLayerName);
        _currentDistance = _defaultDistance;
        _targetDistance = _currentDistance;
        _targetVerticalAngle = 0f;
        _planarDirection = Vector3.forward;
        SetFirstPersonMode(false);
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
    
    public void SetFirstPersonMode(bool firstPersonEnabled)
    {
        if (_originalFirstPersonMask == 0)
        {
            _originalFirstPersonMask = _firstPersonCamera.cullingMask;
        }
        
        _firstPersonCamera.gameObject.SetActive(firstPersonEnabled);
        _thirdPersonCamera.gameObject.SetActive(!firstPersonEnabled);
        _currentActiveCamera = firstPersonEnabled ? _firstPersonCamera : _thirdPersonCamera;
        
        // First-person: Hide player layer only
        _firstPersonCamera.cullingMask = firstPersonEnabled 
            ? _originalFirstPersonMask & ~(1 << _playerLayer)  // Remove player layer
            : _originalFirstPersonMask;                       // Restore original
        
        // Update which camera is active
        _currentActiveCamera = firstPersonEnabled ? _firstPersonCamera : _thirdPersonCamera;
        
        // Update distance for legacy zoom system
        _currentDistance = firstPersonEnabled ? 0f : _defaultDistance;
    }
    public bool IsFirstPerson()
    {
        return _firstPersonCamera.gameObject.activeSelf;
    }
    
    // processes rotation input
    private void HandleRotation(Vector3 rotationInput, out Quaternion targetRotation)
    {
        bool isFirstPerson = IsFirstPerson(); // Check if in first-person mode
        
        if (FindObjectOfType<Interactor>().isRotatingObject && isFirstPerson)
        { 
            _playerTransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            targetRotation = transform.rotation;
            return;
        }

        // In third-person, apply horizontal rotation based on movement input
        _planarDirection = Quaternion.Euler(0, rotationInput.x * _rotationSpeed * mouseSensitivity, 0) * _planarDirection;
        Quaternion planarRot = Quaternion.LookRotation(_planarDirection, Vector3.up);
        
        // Handle Horizontal Rotation (Yaw)
        if (isFirstPerson)
        {
            // In first-person, rotate horizontally with the mouse input
            _playerTransform.rotation = planarRot;
        }

        // Pitch (Vertical Rotation) logic remains the same
        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle - (rotationInput.y * _rotationSpeed * mouseSensitivity), _minVerticalAngle, _maxVerticalAngle);
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
                Mathf.Clamp(hit.distance * 0.2f, _minDistance, _maxDistance),
                1 - Mathf.Exp(-_distanceSharpness * deltaTime));
        }
        else
        {
            _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance,
                1 - Mathf.Exp(-_distanceSharpness * deltaTime));
        }

        // Apply Position
        transform.position = _currentFollowPos - (targetRotation * Vector3.forward * _currentDistance);
    }
    
    private void HandlePickUpCamera()
    {
        bool isFirstPerson = IsFirstPerson();
        _pickupCamera.enabled = isFirstPerson;

        if (isFirstPerson)
        {
            //_currentActiveCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Held"));
        }
        else
        {
            _currentActiveCamera.cullingMask |= (1 << LayerMask.NameToLayer("Held"));
        }
    }
    
    public static class CameraBlocker
    {
        public static bool BlockCameraInput = false;
    }
    
    public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
    {
        if (CameraBlocker.BlockCameraInput)
            return;
        
        if (_followTransform)
        {
            // Store previous state
            bool wasFirstPerson = IsFirstPerson();
        
            // Apply zoom input first
            _targetDistance = Mathf.Clamp(_targetDistance + zoomInput * _distanceSpeed, _minDistance, _maxDistance);

            // Determine if we should switch modes
            bool shouldBeFirstPerson = _targetDistance <= _minDistance + 0.01f; // Small buffer
            bool modeChanged = wasFirstPerson != shouldBeFirstPerson;

            // Only switch modes if we're moving in the appropriate direction
            if (modeChanged)
            {
                if (shouldBeFirstPerson && zoomInput < 0) // Only switch to FP when zooming in
                {
                    SetFirstPersonMode(true);
                }
                else if (!shouldBeFirstPerson && zoomInput > 0) // Only switch to TP when zooming out
                {
                    SetFirstPersonMode(false);
                }
            }

            HandleRotation(rotationInput, out Quaternion targetRotation);
            HandlePosition(deltaTime, 0, targetRotation); // Pass 0 for zoomInput since we already processed it
            HandlePickUpCamera();
        }
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] // variables are editable in unity inspector but remain private
    // Camera distance settings
    private float _defaultDistance = 6f,
        _minDistance = 3f, 
        _maxDistance = 10f,
        _distanceMovementSpeed = 5f,
        _distanceMovementSharpness = 10f, 
        
        // Rotation settings
        _rotationSpeed = 10f,
        _rotationSharpness = 10000f,
        
        // Follow smoothing
        _followSharpness = 10000f, 
        
        // Vertical angle limits (camera up/down)
        _minVerticalAngle = -90f, 
        _maxVerticalAngle = 90f,
        _defaultVerticalAngle = 20f;
    
    // Transform references 
    private Transform _followTransform;
    private Vector3 _currentFollowPosition, _planarDirection;
    
    // Distance and angle tracking
    private float _targetVerticalAngle;
    private float _currentDistance, _targetDistance;

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
        _currentFollowPosition = target.position;
        _planarDirection = target.forward;
    }

    // clamps distance and angle to range set by pre-defined parameters 
    private void OnValidate()
    {
        _defaultDistance = Mathf.Clamp(_defaultDistance, _minDistance, _maxDistance);
        _defaultVerticalAngle = Mathf.Clamp(_defaultVerticalAngle, _minVerticalAngle, _maxVerticalAngle);
    }

    // processes rotation input
    private void HandleRotationInput(float deltaTime, Vector3 rotationInput, out Quaternion targetRotation)
    {
        // Calculates horizontal rotation (Yaw)
        Quaternion rotationFromInput = Quaternion.Euler(_followTransform.up * (rotationInput.x * _rotationSpeed));
        _planarDirection = rotationFromInput * _planarDirection;
        Quaternion planarRot = Quaternion.LookRotation(_planarDirection, _followTransform.up);

        // Calculates vertical rotation (Pitch)
        _targetVerticalAngle -= (rotationInput.y * _rotationSpeed);
        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, _minVerticalAngle, _maxVerticalAngle);
        Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
        
        // Smoothly interpolates to the new rotation
        targetRotation = Quaternion.Slerp(transform.rotation, planarRot * verticalRot, deltaTime * _rotationSharpness);
        transform.rotation = targetRotation;
    }

    private void HandlePosition(float deltaTime, float zoomInput, Quaternion targetRotation)
    {
        // Adjust camera zoom distance
        _targetDistance += zoomInput * _distanceMovementSpeed;
        _targetDistance = Mathf.Clamp(_targetDistance, _minDistance, _maxDistance);
        
        // Smoothly update follow position
        _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, _followTransform.position, 1f - Mathf.Exp(-_followSharpness * deltaTime));
        
        // Calculates camera position
        Vector3 targetPosition = _currentFollowPosition - ((targetRotation * Vector3.forward) * _currentDistance);
        _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, 1 - Mathf.Exp(-_distanceMovementSharpness * deltaTime));
        transform.position = targetPosition;
    }

    public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
    {
        if (_followTransform)
        {
            HandleRotationInput(deltaTime, rotationInput, out Quaternion targetRotation);
            HandlePosition(deltaTime, zoomInput, targetRotation);
        }
    }
}

using KinematicCharacterController;
using Unity.Mathematics.Geometry;
using UnityEngine;

public struct PlayerInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpPressed;
}

public class CharacterController : MonoBehaviour, ICharacterController
{
    [SerializeField]
    private KinematicCharacterMotor _motor;

    [SerializeField]
    private Vector3 _gravity = new Vector3(0f, -30f, 0f); // gravity fall scale
    
    [SerializeField]
    private float _maxStableMoveSpeed = 10f, _stableMovementSharpness = 15f, _orientationSharpness = 10f;

    [SerializeField] 
    private float _jumpSpeed = 10f;
    
    private Vector3 _moveInputVector, _lookInputVector;
    private bool _jumpRequested;
    
    private void Start()
    {
        _motor.CharacterController = this;
    }

    public void SetInputs(ref PlayerInputs inputs)
    {
        Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward),1f);
        Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, _motor.CharacterUp).normalized;

        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, _motor.CharacterUp).normalized;
        }
        
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _motor.CharacterUp);
        
        _moveInputVector = cameraPlanarRotation * moveInputVector;
        _lookInputVector = _moveInputVector.normalized;

        if (inputs.JumpPressed)
        {
            _jumpRequested = true;
        }
        
    }
    
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_lookInputVector.sqrMagnitude > 0f && _orientationSharpness > 0f)
        {
            Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-_orientationSharpness * deltaTime));
            
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
        }
    }
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (_motor.GroundingStatus.IsStableOnGround)
        {
            float currentVelocityMagnitude = currentVelocity.magnitude;
            Vector3 effectiveGroundNormal = _motor.GroundingStatus.GroundNormal;

            currentVelocity = _motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                              currentVelocityMagnitude;

            Vector3 inputRight = Vector3.Cross(_moveInputVector, _motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;

            Vector3 targetMovementVelocity = reorientedInput * _maxStableMoveSpeed;

            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_stableMovementSharpness * deltaTime));
        }
        else
        {
            // gravity call
            currentVelocity += _gravity * deltaTime;
        }

        if (_jumpRequested)
        {
            currentVelocity += (_motor.CharacterUp * _jumpSpeed) - Vector3.Project(currentVelocity, _motor.CharacterUp);
            _jumpRequested = false;
            _motor.ForceUnground();
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public void PostGroundingUpdate(float deltaTime)
    {

    }

    public void AfterCharacterUpdate(float deltaTime)
    {

    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {

    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
        Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {

    }
}

using UnityEngine;
using KinematicCharacterController;
using System;
using Unity.VisualScripting;

public struct PlayerCharacterInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public bool JumpDown;
    public bool DashDown;
}

public class CharacterMovement : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;

    [Header("Stable Movement")]
    public float MaxStableMoveSpeed = 10f;
    public float StableMovementSharpness = 15;
    public float OrientationSharpness = 10;

    [Header("Air Movement")]
    public float MaxAirMoveSpeed = 10f;
    public float AirAccelerationSpeed = 5f;
    public float Drag = 0.1f;

    [Header("Jumping")]
    public bool AllowJumpingWhenSliding = false;
    public float JumpSpeed = 10f;
    public float JumpPreGroundingGraceTime = 0f;
    public float JumpPostGroundingGraceTime = 0f;

    [Header("Dash")]
    public bool AllowDashingWhenSliding = false;
    public bool AllowDashingInAir = false;
    public float DashSpeed = 10f;
    public float DashPreGroundingGraceTime = 0f;
    public float DashPostGroundingGraceTime = 0f;

    [Header("Misc")]
    public bool RotationObstruction;
    public Vector3 Gravity = new Vector3(0, -30f, 0);
    public Transform MeshRoot;

    // Move input
    private Vector3 _moveInputVector;

    // Jump input
    private bool _jumpRequested = false;
    private bool _jumpConsumed = false;
    private bool _jumpedThisFrame = false;
    private float _timeSinceJumpRequested = Mathf.Infinity;
    private float _timeSinceLastAbleToJump = 0f;

    // Dash input
    private bool _dashRequested = false;
    private bool _dashConsumed = false;
    private bool _dashedThisFrame = false;
    private float _timeSinceDashRequested = Mathf.Infinity;
    private float _timeSinceLastAbleToDash = 0f;
    private Vector3 _internalVelocityAdd = Vector3.zero;

    private void Start()
    {
        Motor.CharacterController = this;
    }

    /// <summary>
    /// This is called on input events by the player controller
    /// </summary>
    public void SetInputs(ref PlayerCharacterInputs inputs)
    {
        // Clamp input
        Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

        // Move inputs
        _moveInputVector = moveInputVector;

        // Jumping input
        if (inputs.JumpDown)
        {
            _timeSinceJumpRequested = 0f;
            _jumpRequested = true;
        }

        // Dashing input
        if(inputs.DashDown)
        {
            _timeSinceDashRequested = 0f;
            _dashRequested = true;
        }
    }

    // Begin ICharacterController interface
    public void BeforeCharacterUpdate(float deltaTime)
    {
        // This is called before the motor does anything
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        // This is called when the motor wants to know what its rotation should be right now
        // Smoothly interpolate from current to target look direction
        Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _moveInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

        // Set the current rotation (which will be used by the KinematicCharacterMotor)
        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // This is called when the motor wants to know what its velocity should be right now
        Vector3 targetMovementVelocity = Vector3.zero;
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            // Reorient source velocity on current ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

            // Calculate target velocity
            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
            targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-StableMovementSharpness *  deltaTime));
        }
        else
        {
            // Add move input
            if (_moveInputVector.sqrMagnitude > 0f)
            {
                targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                // Prevent climbing on un-stable slopes with air movement
                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                    targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
            }

            // Gravity
            currentVelocity += Gravity * deltaTime;

            // Drag
            currentVelocity *= (1f / (1f + (Drag * deltaTime)));
        }

        // Handle jumping
        _jumpedThisFrame = false;
        _timeSinceJumpRequested += deltaTime;
        if (_jumpRequested)
        {
            // See if we actually are allowed to jump
            if (!_jumpConsumed
            && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) 
            || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
            {
                // Calculate jump direction before ungrounding
                Vector3 jumpDirection = Motor.CharacterUp;
                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                {
                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                }

                // Makes the character skip ground probing/snapping on its next update. 
                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                Motor.ForceUnground(0.1f);

                // Add to the return velocity and reset jump state
                currentVelocity += (jumpDirection * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                _jumpRequested = false;
                _jumpConsumed = true;
                _jumpedThisFrame = true;
            }
        }

        // Handle dashing
        _dashedThisFrame = false;
        _timeSinceDashRequested += deltaTime;
        if(_dashRequested)
        {
            // See if we actually allowed to dash
            if(!_dashConsumed 
            && ((AllowDashingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
            || AllowDashingInAir  
            || _timeSinceLastAbleToDash <= DashPostGroundingGraceTime))
            {
                // Makes the character skip ground probing/snapping on its next update.
                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                Motor.ForceUnground(0.1f);

                currentVelocity += _moveInputVector * DashSpeed;
                _dashRequested = false;
                _dashConsumed = true;
                _dashedThisFrame = true;
            }
        }

        // Take into account additive velocity
        if (_internalVelocityAdd.sqrMagnitude > 0f)
        {
            currentVelocity += _internalVelocityAdd;
            _internalVelocityAdd = Vector3.zero;
        }

    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        // This is called after the motor has finished everything in its update
        // Handle jump-related values
        {
            // Handle jumping pre-ground grace period
            if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
            {
                _jumpRequested = false;
            }

            // Handle jumping while sliding
            if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
            {
                // If we're on a ground surface, reset jumping values
                if (!_jumpedThisFrame)
                {
                    _jumpConsumed = false;
                }
                _timeSinceLastAbleToJump = 0f;
            }
            else
            {
                // Keep track of time since we were last able to jump (for grace period)
                _timeSinceLastAbleToJump += deltaTime;
            }
        }

        // Handle dash-related values
        {
            // Handle dashing pre-ground grace period
            if(_dashRequested && _timeSinceDashRequested > DashPreGroundingGraceTime)
            {
                _dashRequested = false;
            }

            // Handle dashing while sliding/in air
            if ((AllowDashingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || AllowDashingInAir)
            {
                if (!_dashedThisFrame)
                {
                    _dashConsumed = false;
                }
                _timeSinceLastAbleToDash = 0f;
            }
            else
            {
                // Keep track of time since we were last able to jump (for grace period)
                _timeSinceLastAbleToDash += deltaTime;
            }
        }
        
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        // This is called after when the motor wants to know if the collider can be collided with (or if we just go through it)
        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // This is called when the motor's ground probing detects a ground hit
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // This is called when the motor's movement logic detects a hit
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
        // This is called after every hit detected in the motor, to give you a chance to modify the HitStabilityReport any way you want
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        // This is called after the motor has finished its ground probing, but before PhysicsMover/Velocity/etc.... handling
    }

    public void AddVelocity(Vector3 velocity)
    {
        _internalVelocityAdd += velocity;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
        // This is called by the motor when it is detecting a collision that did not result from a "movement hit".
    }
    // End ICharacterController interface
}


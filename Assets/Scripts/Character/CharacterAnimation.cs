using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private enum Melee_Phase
    {
        Melee_A,
        Melee_B,
    }

    [Header("Components")]
    public CharacterMovement Movement;
    public KinematicCharacterMotor Motor;
    public Animator animator;

    [Header("Melee")]
    public float MeleeExpiryTime = 1f;
    private Melee_Phase _MeleePhase = Melee_Phase.Melee_A;
    private Coroutine _MeleeTimer;

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = Movement.MoveInputVector;

        if(Motor.Velocity.magnitude != 0)
            animator.SetFloat("Velocity", moveDirection.magnitude);
        else
            animator.SetFloat("Velocity", 0);

        if(moveDirection.magnitude != 0)
        {
            animator.SetFloat("MoveX", moveDirection.x);
            animator.SetFloat("MoveY", moveDirection.z);
        }

        animator.SetBool("Jump", Movement.JumpConsumed && !Motor.GroundingStatus.IsStableOnGround);
        animator.SetFloat("VVelocity", Motor.Velocity.y);

        animator.SetBool("Dash", Movement.DashConsumed && moveDirection.magnitude != 0);
    }

    public void Melee()
    {
        switch (_MeleePhase)
        {
            case Melee_Phase.Melee_A:
                _MeleePhase = Melee_Phase.Melee_B;
                break;
            default:
                _MeleePhase = Melee_Phase.Melee_A;
                break;
        }

        animator.Play(Enum.GetName(typeof(Melee_Phase), _MeleePhase));

        if (_MeleeTimer != null)
        {
            StopCoroutine(_MeleeTimer);
        }

        _MeleeTimer = StartCoroutine(ResetMeleeCombo());
    }

    private IEnumerator ResetMeleeCombo()
    {
        yield return new WaitForSeconds(MeleeExpiryTime);
        _MeleePhase = Melee_Phase.Melee_A;
    }
}

using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public CharacterMovement Movement;
    public KinematicCharacterMotor Motor;
    public Animator animator;

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
}

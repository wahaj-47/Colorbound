using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    private Vector2 _moveDirection;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Velocity", _moveDirection.magnitude);

        if(_moveDirection.magnitude != 0)
        {
            animator.SetFloat("MoveX", _moveDirection.x);
            animator.SetFloat("MoveY", _moveDirection.y);
        }
    }

    public void SetParams(ref PlayerCharacterInputs animationParams)
    {
        // Clamp input
        Vector2 moveDirection = Vector2.ClampMagnitude(new Vector2(animationParams.MoveAxisRight, animationParams.MoveAxisForward), 1f);
        _moveDirection = moveDirection;
    }
}

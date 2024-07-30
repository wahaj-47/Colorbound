using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIController : MonoBehaviour
{
    private NavMeshAgent Agent;
    private CharacterMovement CharacterMovement;
    private CharacterAnimation CharacterAnimation;
    private PlayerCharacterInputs _characterInputs;
    private AbilityManager CharacterAbilityManager;

    private void OnEnable()
    {
        if(!gameObject.TryGetComponent<NavMeshAgent>(out var outAgent))
        {
            Debug.Log("Missing navigation component");
            return;
        }

        if(!gameObject.TryGetComponent<CharacterMovement>(out var outCharacterMovement))
        {
            Debug.Log("Missing movement component");
            return;
        }

        if(!gameObject.TryGetComponent<CharacterAnimation>(out var outCharacterAnimation))
        {
            Debug.Log("Animation component missing");
            return;
        }

        if(!gameObject.TryGetComponent<AbilityManager>(out var outCharacterAbilityManager))
        {
            Debug.Log("Missing ability system component");
            return;
        }

        Agent = outAgent;
        CharacterMovement = outCharacterMovement;
        CharacterAnimation = outCharacterAnimation;
        CharacterAbilityManager = outCharacterAbilityManager;
    }

    private void Start()
    {
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        _characterInputs = new PlayerCharacterInputs();
    }

    private void Update()
    {
        Agent.nextPosition = transform.position;
        Move();
    }

    public void Move()
    {
        Vector3 moveDirection = Agent.velocity.magnitude < 1 ? Vector3.zero : Agent.velocity;

        // Build the CharacterInputs struct
        _characterInputs.MoveAxisForward =  moveDirection.z;
        _characterInputs.MoveAxisRight =  moveDirection.x;

        // Apply inputs to character
        CharacterMovement.SetInputs(ref _characterInputs);
        // Apply inputs to animator
        CharacterAnimation.SetParams(ref _characterInputs);
    }

    public void Jump()
    {
        _characterInputs.JumpDown = true;

        // Apply inputs to character
        CharacterMovement.SetInputs(ref _characterInputs);

        // Reset input 
        // This is to force the player to trigger the action again
        _characterInputs.JumpDown = false;
        CharacterMovement.SetInputs(ref _characterInputs);
    }

    public void Dash()
    {
        _characterInputs.DashDown = true;

        // Apply inputs to character
        CharacterMovement.SetInputs(ref _characterInputs);

        // Reset input 
        // This is to force the player to trigger the action again
        _characterInputs.DashDown = false;
        CharacterMovement.SetInputs(ref _characterInputs);
    }

    public void Hunt()
    {
        CharacterAbilityManager.Hunt();
    }
    public void AbilityOne()
    {
        CharacterAbilityManager.Perform(AbilityManager.EAbility.One);
    }

    public void AbilityTwo()
    {
        CharacterAbilityManager.Perform(AbilityManager.EAbility.Two);
    }

    public void AbiliyThree()
    {
        CharacterAbilityManager.Perform(AbilityManager.EAbility.Three);
    }

    public void AbilityFour()
    {
        CharacterAbilityManager.Perform(AbilityManager.EAbility.Four);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using AttributeSystem.Authoring;
using AttributeSystem.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIStateMachine : BaseStateMachine
{
    [HideInInspector]
    public Collider target;
    
    [HideInInspector]
    public Vector3 spawnPosition;

    [Header("Controller")]
    public AIController Controller;

    [Header("Attributes")]
    public AttributeSystemComponent attributeSystemComponent;
    public AttributeScriptableObject scanInterval, sightRange, viewAngle, patrolRadius;

    [Header("Abilities")]
    public AbilityManager abilityManagerComponent;

    [Header("Definitions")]
    public LayerMask whatIsTarget;
    public LayerMask whatIsObstacle;

    public AIState PatrolState;
    public AIState CombatState;

    public override void OnEnable()
    {
        base.OnEnable();

        spawnPosition = transform.position;

        PatrolState = new AIPatrolState(this);
        CombatState = new AICombatState(this);
    }

    public AttributeValue GetAttributeValue(AttributeScriptableObject attribute)
    {
        AttributeValue value;
        attributeSystemComponent.GetAttributeValue(attribute, out value);
        return value;
    }

    public override BaseState GetInitialState()
    {
        return PatrolState;
    }

}

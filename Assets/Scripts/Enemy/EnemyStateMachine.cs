using System;
using System.Collections;
using System.Collections.Generic;
using AttributeSystem.Authoring;
using AttributeSystem.Components;
using UnityEngine;

public class EnemyStateMachine : BaseStateMachine
{
    public Vector3 spawnPosition;
    public AttributeSystemComponent attributeSystemComponent;
    public AttributeScriptableObject scanInterval, sightRange, viewAngle, patrolRadius;
    public LayerMask whatIsPlayer, whatIsObstacle;

    public EnemyState PatrolState;
    public EnemyState AttackState;

    private void Awake()
    {
        spawnPosition = transform.position;
        PatrolState = new EnemyPatrolState(this);
        AttackState = new EnemyAttackState(this);
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

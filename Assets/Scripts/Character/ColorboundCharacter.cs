using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AbilitySystemCharacter), typeof(NavMeshAgent))]
public class ColorboundCharacter : MonoBehaviour, IDamageable
{
    [Header("Type")]
    public TypeTagScriptableObject type;
    public TypeTagScriptableObject TypeTag => type;

    private AbilitySystemCharacter AbilitySystemComponent;
    private NavMeshAgent NavMeshAgent;

    private void Awake()
    {
        AbilitySystemComponent = GetComponent<AbilitySystemCharacter>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;
    }

    private void Update()
    {
        NavMeshAgent.nextPosition = transform.position;
    }

    public void Damage(GameplayEffectScriptableObject damageEffect, GameObject instigator)
    {
        GameplayEffectSpec damageEffectSpec = AbilitySystemComponent.MakeOutgoingSpec(damageEffect);
        AbilitySystemComponent.ApplyGameplayEffectSpecToSelf(damageEffectSpec);
    }
}

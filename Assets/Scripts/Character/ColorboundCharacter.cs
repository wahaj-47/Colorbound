using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AbilitySystemCharacter))]
public class ColorboundCharacter : MonoBehaviour, IDamageable
{
    [Header("Type")]
    public GameplayTagScriptableObject type;
    public GameplayTagScriptableObject TypeTag => type;

    [Header("Masks")]
    public LayerMask whatIsPlayer;

    private AbilitySystemCharacter AbilitySystemComponent;

    private void Awake()
    {
        AbilitySystemComponent = GetComponent<AbilitySystemCharacter>();
    }

    public void Damage(GameplayEffectScriptableObject damageEffect, GameObject instigator)
    {
        GameplayEffectSpec damageEffectSpec = AbilitySystemComponent.MakeOutgoingSpec(damageEffect);
        AbilitySystemComponent.ApplyGameplayEffectSpecToSelf(damageEffectSpec);
    }
}

using System;
using System.Collections;
using AbilitySystem;
using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(AbilitySystemCharacter))]
public class AbilityManager : MonoBehaviour, IDamageable
{
    public enum EAbility {One, Two, Three, Four};

    [Header("Type")]
    public GameplayTagScriptableObject type;

    [Header("Masks")]
    public LayerMask whatIsPlayer;

    [Header("Attacks")]
    public MeleeAbilityScriptableObject abilityOne;
    public RangedAbilityScriptableObject abilityTwo;
    public MeleeAbilityScriptableObject abilityThree_A;
    public MeleeAbilityScriptableObject abilityThree_B;
    public MeleeAbilityScriptableObject abilityThree_C;
    public MeleeAbilityScriptableObject abilityFour;

    [Header("Events")]
    public UnityEvent attackEvent;

    [Header("Passive abilities")]
    public AbstractAbilityScriptableObject hunt;

    private AbilitySystemCharacter AbilitySystemComponent;
    public GameplayTagScriptableObject typeTag { get => type; }

    private void Awake()
    {
        AbilitySystemComponent = GetComponent<AbilitySystemCharacter>();
    }

    private void Start()
    {
        if(attackEvent == null) attackEvent = new UnityEvent();
    }

    public void Perform(EAbility attack)
    {
        AbstractAbilitySpec abilitySpec;

        switch (attack)
        {
            case EAbility.One:
                abilitySpec = abilityOne ? abilityOne.CreateSpec(AbilitySystemComponent) : null;
                break;
            case EAbility.Two:
                abilitySpec = abilityTwo ? abilityTwo.CreateSpec(AbilitySystemComponent) : null;
                break;
            case EAbility.Three:
                // @TODO: Decide which ability to activate based on active gameplay tags
                abilitySpec = abilityThree_A ? abilityThree_A.CreateSpec(AbilitySystemComponent) : null;
                break;
            case EAbility.Four:
                abilitySpec = abilityFour ? abilityFour.CreateSpec(AbilitySystemComponent) : null;
                break;
            default:
                abilitySpec = abilityOne ? abilityOne.CreateSpec(AbilitySystemComponent) : null;
                break;
        }

        if(abilitySpec != null)
            StartCoroutine(abilitySpec.TryActivateAbility());
    }

    public void Hunt()
    {
        AbstractAbilitySpec abilitySpec = hunt.CreateSpec(AbilitySystemComponent);
        StartCoroutine(abilitySpec.TryActivateAbility());
    }

    public void Damage(GameplayEffectScriptableObject damageEffect, GameObject instigator)
    {
        GameplayEffectSpec damageEffectSpec = AbilitySystemComponent.MakeOutgoingSpec(damageEffect);
        AbilitySystemComponent.ApplyGameplayEffectSpecToSelf(damageEffectSpec);
        
        if((whatIsPlayer.value & (1 << gameObject.layer)) > 0)
            StartCoroutine(Immune());
    }

    private IEnumerator Immune()
    {
        if(TryGetComponent<NavMeshObstacle>(out var navMeshObstacle))
        {
            navMeshObstacle.enabled = true;
            yield return new WaitForSeconds(0.7f);
            navMeshObstacle.enabled = false;
        }

        yield break;
    }

}

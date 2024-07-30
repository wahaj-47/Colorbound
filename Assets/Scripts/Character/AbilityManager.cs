using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(AbilitySystemCharacter))]
public class AbilityManager : MonoBehaviour, IDamageable
{
    public enum EAbility {One, Two, Three, Four};

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
                abilitySpec = abilityOne.CreateSpec(AbilitySystemComponent);
                break;
            case EAbility.Two:
                abilitySpec = abilityTwo.CreateSpec(AbilitySystemComponent);
                break;
            case EAbility.Three:
                // @TODO: Decide which ability to activate based on active gameplay tags
                abilitySpec = abilityThree_A.CreateSpec(AbilitySystemComponent);
                break;
            case EAbility.Four:
                abilitySpec = abilityFour.CreateSpec(AbilitySystemComponent);
                break;
            default:
                abilitySpec = abilityOne.CreateSpec(AbilitySystemComponent);
                break;
        }

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

        if(tag == "Player")
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

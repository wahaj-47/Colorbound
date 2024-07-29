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
    [Header("Attacks")]
    public MeleeAbilityScriptableObject melee;
    public MeleeAbilityScriptableObject ranged;
    public MeleeAbilityScriptableObject dual_A;
    public MeleeAbilityScriptableObject dual_B;
    public MeleeAbilityScriptableObject triple;

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

    public void Hunt()
    {
        AbstractAbilitySpec abilitySpec = hunt.CreateSpec(AbilitySystemComponent);
        StartCoroutine(abilitySpec.TryActivateAbility());
    }

    public void Melee()
    {
        attackEvent.Invoke();
        
        AbstractAbilitySpec abilitySpec = melee.CreateSpec(AbilitySystemComponent);
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

using System;
using System.Collections;
using AbilitySystem;
using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(AbilitySystemCharacter))]
public class AbilityManager : MonoBehaviour
{
    public enum EAbility {One, Two, Three, Four};

    [Header("Attacks")]
    public MeleeAbilityScriptableObject abilityOne;
    public RangedAbilityScriptableObject abilityTwo;
    public MeleeAbilityScriptableObject[] abilityThree;
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
                abilitySpec = abilityOne ? abilityOne.CreateSpec(AbilitySystemComponent) : null;
                break;
            case EAbility.Two:
                abilitySpec = abilityTwo ? abilityTwo.CreateSpec(AbilitySystemComponent) : null;
                break;
            
            // Executing all possible abilities.
            case EAbility.Three:
                for (int i = 0; i < abilityThree.Length; i++)
                {
                    abilitySpec = abilityThree[i].CreateSpec(AbilitySystemComponent);
                    StartCoroutine(abilitySpec.TryActivateAbility());
                }
                return;

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

}

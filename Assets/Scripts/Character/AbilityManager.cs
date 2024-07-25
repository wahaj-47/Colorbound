using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;

[RequireComponent(typeof(AbilitySystemCharacter))]
public class AbilityManager : MonoBehaviour
{
    public MeleeAbilityScriptableObject melee;
    public MeleeAbilityScriptableObject ranged;
    public MeleeAbilityScriptableObject dual_A;
    public MeleeAbilityScriptableObject dual_B;
    public MeleeAbilityScriptableObject triple;

    private AbilitySystemCharacter AbilitySystemComponent;

    private void Awake()
    {
        AbilitySystemComponent = GetComponent<AbilitySystemCharacter>();
    }

    public void Melee()
    {
        AbstractAbilitySpec abilitySpec = melee.CreateSpec(AbilitySystemComponent);
        StartCoroutine(abilitySpec.TryActivateAbility());
    }
}

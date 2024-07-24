using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;

public class InitStats : MonoBehaviour
{
     [SerializeField] private AbstractAbilityScriptableObject ability;
    [SerializeField] private AbilitySystemCharacter abilitySystemCharacter;
   
    void Awake()
    {
        AbstractAbilitySpec abilitySpec = ability.CreateSpec(abilitySystemCharacter);
        StartCoroutine(abilitySpec.TryActivateAbility());
    }
}

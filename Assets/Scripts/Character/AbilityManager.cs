using System.ComponentModel;
using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;


[RequireComponent(typeof(AbilitySystemCharacter))]
public class AbilityManager : MonoBehaviour
{
    [Header("Abilities")]
    [SerializeField] private AbilitySetScriptableObject AbilitySet;
    private AbilitySystemCharacter AbilitySystemComponent;

    private void Awake()
    {
        AbilitySystemComponent = GetComponent<AbilitySystemCharacter>();
    }

    public void AssignAbilitySet(AbilitySetScriptableObject abilitySet)
    {
        AbilitySet = abilitySet;
    }

    public void Perform(EAbility key)
    {
        if (AbilitySet.ActiveAbilities.TryGetValue(key, out AbstractAbilityScriptableObject ability))
        {
            AbstractAbilitySpec abilitySpec = ability.CreateSpec(AbilitySystemComponent);
            StartCoroutine(abilitySpec.TryActivateAbility());
        }
    }

}

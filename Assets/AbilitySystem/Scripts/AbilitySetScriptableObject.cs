using System.Collections.Generic;
using AbilitySystem.Authoring;
using AYellowpaper.SerializedCollections;
using GameplayTag.Authoring;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Ability Set")]
public class AbilitySetScriptableObject : ScriptableObject
{
    public SerializedDictionary<EAbility, AbstractAbilityScriptableObject> ActiveAbilities;
    public List<AbstractAbilityScriptableObject> PassiveAbilities;
}

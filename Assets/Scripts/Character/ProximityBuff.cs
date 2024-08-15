using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using AYellowpaper.SerializedCollections;
using GameplayTag.Authoring;
using UnityEngine;

public class ProximityBuff : MonoBehaviour
{
    public GameplayTagScriptableObject tags;
    public AbilitySystemCharacter AbilitySystemComponent;
    public SerializedDictionary<GameplayTagScriptableObject, GameplayEffectScriptableObject> Buffs;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            if(Buffs.TryGetValue(damageable.TypeTag, out var buffEffect))
            {
                GameplayEffectSpec buffEffectSpec = AbilitySystemComponent.MakeOutgoingSpec(buffEffect);
                AbilitySystemComponent.ApplyGameplayEffectSpecToSelf(buffEffectSpec);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            if(Buffs.TryGetValue(damageable.TypeTag, out var buffEffect))
            {
                for (int i = 0; i < AbilitySystemComponent.AppliedGameplayEffects.Count; i++)
                {
                    if(AbilitySystemComponent.AppliedGameplayEffects[i].spec.GameplayEffect.gameplayEffectTags.AssetTag == buffEffect.gameplayEffectTags.AssetTag)
                    {
                        AbilitySystemComponent.AppliedGameplayEffects.RemoveAt(i);
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Authoring;
using AYellowpaper.SerializedCollections;
using GameplayTag.Authoring;
using UnityEngine;

public static class ExtensionMethod
{
    public static Object Instantiate(
        this Object thisObj, 
        Object original, 
        Vector3 position, 
        Quaternion rotation, 
        ref SerializedDictionary<GameplayTagScriptableObject, GameplayEffectScriptableObject> damageEffects, 
        GameObject instigator, 
        LayerMask layerMask)
    {
        GameObject particleSystem = Object.Instantiate(original, position, rotation) as GameObject;

        if(particleSystem.TryGetComponent<ParticleDamage>(out var component))
        {
            component.damageEffects = damageEffects;
            component.instigator = instigator;
            component.whatIsDamaged = layerMask;
        }
        return particleSystem;
    }
}

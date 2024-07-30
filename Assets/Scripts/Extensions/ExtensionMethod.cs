using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Authoring;
using UnityEngine;

public static class ExtensionMethod
{
    public static Object Instantiate(this Object thisObj, Object original, Transform parent, bool instantiateInWorldSpace, GameplayEffectScriptableObject damageEffect, GameObject instigator, LayerMask layerMask)
    {
        GameObject particleSystem = Object.Instantiate(original, parent, instantiateInWorldSpace) as GameObject;

        particleSystem.transform.localPosition = Vector3.zero;
        particleSystem.transform.localRotation = Quaternion.identity;

        if(particleSystem.TryGetComponent<ParticleDamage>(out var component))
        {
            component.damageEffect = damageEffect;
            component.instigator = instigator;
            component.whatIsDamaged = layerMask;
        }
        return particleSystem;
    }
}

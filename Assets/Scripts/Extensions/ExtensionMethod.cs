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

        if (particleSystem.TryGetComponent<ParticleDamage>(out var component))
        {
            component.damageEffects = damageEffects;
            component.instigator = instigator;
            component.whatIsDamaged = layerMask;
        }
        return particleSystem;
    }

    public static bool CanSee(
        this GameObject gameObject,
        Transform target,
        float viewAngle,
        LayerMask obstacleLayerMask,
        out RaycastHit hitInfo
    )
    {
        hitInfo = new RaycastHit();
        Vector3 direction = target.position - gameObject.transform.position;
        Vector3 offset = new Vector3(0, 1, 0);

        if (Vector3.Angle(gameObject.transform.forward, direction.normalized) < viewAngle / 2)
        {
            if (!Physics.Raycast(gameObject.transform.position + offset, direction + offset, out RaycastHit hit, obstacleLayerMask))
            {
                hitInfo = hit;
                return true;
            }
        }

        return false;
    }
}

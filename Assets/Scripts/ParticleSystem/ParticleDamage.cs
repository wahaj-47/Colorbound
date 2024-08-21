using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDamage : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<GameplayTagScriptableObject, GameplayEffectScriptableObject> damageEffects;

    [HideInInspector]
    public GameObject instigator;

    [HideInInspector]
    public LayerMask whatIsDamaged;

    private void OnParticleCollision(GameObject other)
    {
        if ((whatIsDamaged.value & (1 << other.layer)) > 0 && other.TryGetComponent<ColorboundCharacter>(out var damageable))
        {
            if (damageable.TypeTag != null && damageEffects.TryGetValue(damageable.TypeTag, out GameplayEffectScriptableObject damageEffect))
            {
                if (other.TryGetComponent<CharacterMovement>(out var CharacterMovementComponent))
                {
                    // Adding impulse to the hit object
                    CharacterMovementComponent.AddVelocity(gameObject.transform.forward * 25.0f);
                }

                damageable.Damage(damageEffect, instigator);
            }
        }
    }
}

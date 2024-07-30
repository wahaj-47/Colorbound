using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Authoring;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDamage : MonoBehaviour
{
    private ParticleSystem particleSystem;

    [HideInInspector]
    public GameplayEffectScriptableObject damageEffect;
    
    [HideInInspector]
    public GameObject instigator;
    
    [HideInInspector]
    public LayerMask whatIsDamaged;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    } 
    private void OnParticleCollision(GameObject other)
    {
        if((whatIsDamaged.value & (1 << other.layer)) > 0 && other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(damageEffect, instigator);
        }
    }
}
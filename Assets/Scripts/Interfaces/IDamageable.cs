using AbilitySystem.Authoring;
using UnityEngine;

public interface IDamageable
{
    public TypeTagScriptableObject TypeTag { get; }
    public void Damage(GameplayEffectScriptableObject damageEffect, GameObject instigator);
}
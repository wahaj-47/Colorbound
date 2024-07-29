using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Authoring;
using UnityEngine;

public interface IDamageable
{
    void Damage(GameplayEffectScriptableObject damageEffect, GameObject instigator);
}
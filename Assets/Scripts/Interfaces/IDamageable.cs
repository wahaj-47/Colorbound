using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Authoring;
using GameplayTag.Authoring;
using UnityEngine;

public interface IDamageable
{
    public GameplayTagScriptableObject TypeTag {get;}
    void Damage(GameplayEffectScriptableObject damageEffect, GameObject instigator);
}
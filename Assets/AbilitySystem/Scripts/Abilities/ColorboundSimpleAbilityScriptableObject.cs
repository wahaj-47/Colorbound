using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Colorbound Simple Ability")]
public class ColorboundSimpleAbilityScriptableObject : SimpleAbilityScriptableObject
{
    public class ColorboundSimpleAbilitySpec : SimpleAbilitySpec
    {
        public ColorboundSimpleAbilitySpec(AbstractAbilityScriptableObject abilitySO, AbilitySystemCharacter owner)
            : base(abilitySO, owner)
        {
        }

        public override void CancelAbility()
        {
            this.Owner.RemoveAbilitiesWithTag(this.Ability.AbilityTags.AssetTag);
            this.Owner.AppliedGameplayEffects.RemoveAll
            (
                (GameplayEffectContainer x) =>
                    x.spec.GameplayEffect.gameplayEffectTags.AssetTag
                    ==
                    (this.Ability as ColorboundSimpleAbilityScriptableObject).GameplayEffect.gameplayEffectTags.AssetTag
            );
        }
    }
}

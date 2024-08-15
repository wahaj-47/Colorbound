using System;
using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using AYellowpaper.SerializedCollections;
using GameplayTag.Authoring;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Ranged Attack Ability")]
public class RangedAbilityScriptableObject : AbstractAbilityScriptableObject
{
    public SerializedDictionary<GameplayTagScriptableObject, GameplayEffectScriptableObject> Damage;
    public GameObject AttackPrefab;
    public LayerMask Layers;
    /// <summary>
    /// Gameplay Effect to apply
    /// </summary>
    public GameplayEffectScriptableObject GameplayEffect;

    /// <summary>
    /// Creates the Ability Spec, which is instantiated for each character.
    /// </summary>
    /// <param name="owner"></param>
    /// <returns></returns>
    public override AbstractAbilitySpec CreateSpec(AbilitySystemCharacter owner)
    {
        var spec = new RangedAbilitySpec(this, owner);
        spec.Level = owner.Level;
        return spec;
    }

    /// <summary>
    /// The Ability Spec is the instantiation of the ability.  Since the Ability Spec
    /// is instantiated for each character, we can store stateful data here.
    /// </summary>
    public class RangedAbilitySpec : AbstractAbilitySpec
    {

        private Transform _attackPoint;

        public RangedAbilitySpec(AbstractAbilityScriptableObject abilitySO, AbilitySystemCharacter owner) : base(abilitySO, owner)
        {
            _attackPoint = Owner.transform.Find("AttackPoint_Melee");
            if(_attackPoint == null)
            {
                Debug.Log("Failed to find attack point");
            }
        }

        /// <summary>
        /// What to do when the ability is cancelled.  We don't care about there for this example.
        /// </summary>
        public override void CancelAbility() 
        {
        }

        /// <summary>
        /// What happens when we activate the ability.
        /// 
        /// In this example, we apply the cost and cooldown, and then we apply the main
        /// gameplay effect
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator ActivateAbility()
        {
            // Apply cost and cooldown
            var cdSpec = this.Owner.MakeOutgoingSpec(this.Ability.Cooldown);
            var costSpec = this.Owner.MakeOutgoingSpec(this.Ability.Cost);
            this.Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
            this.Owner.ApplyGameplayEffectSpecToSelf(costSpec);


            // Apply primary effect
            var effectSpec = this.Owner.MakeOutgoingSpec((this.Ability as RangedAbilityScriptableObject).GameplayEffect);
            this.Owner.ApplyGameplayEffectSpecToSelf(effectSpec);

            if(_attackPoint == null)
            {
                Debug.Log("Missing attack point");
                yield break;
            }

            if(this.Owner.TryGetComponent<CharacterMovement>(out var CharacterMovementComponent))
            {
                // Adding impulse to the hit object
                CharacterMovementComponent.AddVelocity(Owner.transform.forward * -10.0f);
            }

            if(this.Owner.TryGetComponent<CharacterAnimation>(out var characterAnimation))
            {
                characterAnimation.Melee();
            }

            this.Owner.Instantiate(
                (this.Ability as RangedAbilityScriptableObject).AttackPrefab, // Particle effect to spawn
                _attackPoint.transform.position, // Instigator position
                _attackPoint.transform.rotation, // Instigator rotation
                ref (this.Ability as RangedAbilityScriptableObject).Damage, // Damage effect to apply
                this.Owner.gameObject, // Instigator
                (this.Ability as RangedAbilityScriptableObject).Layers // Layer mask for detecting collisions
            );

            yield return null;
        }

        /// <summary>
        /// Checks to make sure Gameplay Tags checks are met. 
        /// 
        /// Since the target is also the character activating the ability,
        /// we can just use Owner for all of them.
        /// </summary>
        /// <returns></returns>
        public override bool CheckGameplayTags()
        {
            return AscHasAllTags(Owner, this.Ability.AbilityTags.OwnerTags.RequireTags)
                    && AscHasNoneTags(Owner, this.Ability.AbilityTags.OwnerTags.IgnoreTags)
                    && AscHasAllTags(Owner, this.Ability.AbilityTags.SourceTags.RequireTags)
                    && AscHasNoneTags(Owner, this.Ability.AbilityTags.SourceTags.IgnoreTags)
                    && AscHasAllTags(Owner, this.Ability.AbilityTags.TargetTags.RequireTags)
                    && AscHasNoneTags(Owner, this.Ability.AbilityTags.TargetTags.IgnoreTags);
        }

        /// <summary>
        /// Logic to execute before activating the ability.  We don't need to do anything here
        /// for this example.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PreActivate()
        {
            yield return null;
        }
    }
}

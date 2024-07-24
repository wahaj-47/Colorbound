using System;
using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using AbilitySystem.Authoring;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Colorbound Ability")]
public class MeleeAbilityScriptableObject : AbstractAbilityScriptableObject
{
    public float Damage;
    public float Range;
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
        var spec = new MeleeAbilitySpec(this, owner);
        spec.Level = owner.Level;
        return spec;
    }

    /// <summary>
    /// The Ability Spec is the instantiation of the ability.  Since the Ability Spec
    /// is instantiated for each character, we can store stateful data here.
    /// </summary>
    public class MeleeAbilitySpec : AbstractAbilitySpec
    {

        private Transform _attackPoint;

        public MeleeAbilitySpec(AbstractAbilityScriptableObject abilitySO, AbilitySystemCharacter owner) : base(abilitySO, owner)
        {
            _attackPoint = owner.transform.Find("AttackPoint_Melee");
        }

        /// <summary>
        /// What to do when the ability is cancelled.  We don't care about there for this example.
        /// </summary>
        public override void CancelAbility() { }

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
            var effectSpec = this.Owner.MakeOutgoingSpec((this.Ability as MeleeAbilityScriptableObject).GameplayEffect);
            this.Owner.ApplyGameplayEffectSpecToSelf(effectSpec);

            Attack();

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

        public virtual void Attack()
        {
            Collider[] outHits = Physics.OverlapSphere(_attackPoint.position, (this.Ability as MeleeAbilityScriptableObject).Range, (this.Ability as MeleeAbilityScriptableObject).Layers);
            foreach (Collider hitObject in outHits)
            {
                (hitObject as IDamageable).Damage((this.Ability as MeleeAbilityScriptableObject).Damage);
            }
        }
    }
}

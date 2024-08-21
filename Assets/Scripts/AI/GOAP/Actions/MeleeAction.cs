using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

public class MeleeAction : ActionBase<AttackData>, IInjectable
{
    private AttackConfigSO AttackConfig;
    public override void Created() { }

    public override void Start(IMonoAgent agent, AttackData data)
    {
        data.Timer = AttackConfig.MeleeAttackDelay;
    }

    public override ActionRunState Perform(IMonoAgent agent, AttackData data, ActionContext context)
    {
        data.Timer -= context.DeltaTime;

        if (data.Target == null) return ActionRunState.Stop;

        bool shouldAttack = data.Target != null
                         && Vector3.Distance(data.Target.Position, agent.transform.position)
                         <= AttackConfig.MeleeAttackRadius;

        if (shouldAttack)
        {
            data.AbilityManager.Perform(EAbility.Melee);
        }

        return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
    }

    public override void End(IMonoAgent agent, AttackData data) { }

    public void Inject(DependencyInjector injector)
    {
        AttackConfig = injector.AttackConfig;
    }
}

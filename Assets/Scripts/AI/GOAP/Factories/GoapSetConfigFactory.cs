using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using UnityEngine;

[RequireComponent(typeof(DependencyInjector))]
public class GoapSetConfigFactory : GoapSetFactoryBase
{
    private DependencyInjector Injector;
    public override IGoapSetConfig Create()
    {
        Injector = GetComponent<DependencyInjector>();
        GoapSetBuilder builder = new("EnemySet");

        BuildGoals(builder);
        BuildActions(builder);
        BuildSensors(builder);

        return builder.Build();
    }

    private void BuildGoals(GoapSetBuilder builder)
    {
        builder.AddGoal<WanderGoal>()
            .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

        builder.AddGoal<KillPlayerGoal>()
            .AddCondition<TargetHealth>(Comparison.SmallerThanOrEqual, 0);
    }

    private void BuildActions(GoapSetBuilder builder)
    {
        builder.AddAction<WanderAction>()
            .SetTarget<WanderTarget>()
            .AddEffect<IsWandering>(EffectType.Increase)
            .SetBaseCost(1)
            .SetInRange(0.5f);

        builder.AddAction<MeleeAction>()
            .SetTarget<DestroyTarget>()
            .AddEffect<TargetHealth>(EffectType.Decrease)
            .SetBaseCost(Injector.AttackConfig.MeleeAttackCost)
            .SetInRange(Injector.AttackConfig.SensorRadius);
    }

    private void BuildSensors(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget<WanderTarget>();

        builder.AddTargetSensor<DestroyTargetSensor>()
            .SetTarget<DestroyTarget>();
    }
}

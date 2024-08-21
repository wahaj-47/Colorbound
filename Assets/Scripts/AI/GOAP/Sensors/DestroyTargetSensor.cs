using System;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

public class DestroyTargetSensor : LocalTargetSensorBase, IInjectable
{
    private AttackConfigSO AttackConfig;
    public override void Created() { }

    public override void Update() { }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        AgentBrain brain = references.GetCachedComponent<AgentBrain>();
        LOSSensor sensor = brain.LOSSensor;

        if (sensor.TargetsInSight.Count > 0 && sensor.TargetsInSight[0] != null)
            return new TransformTarget(sensor.TargetsInSight[0].transform);

        return null;
    }

    public void Inject(DependencyInjector injector)
    {
        AttackConfig = injector.AttackConfig;
    }
}

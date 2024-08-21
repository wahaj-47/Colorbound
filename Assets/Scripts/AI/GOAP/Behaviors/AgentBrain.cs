using System;
using System.Collections;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

[RequireComponent(typeof(AgentBehaviour))]
public class AgentBrain : MonoBehaviour
{
    public LOSSensor LOSSensor;
    [SerializeField] private AttackConfigSO AttackConfig;
    private AgentBehaviour AgentBehaviour;

    private void Awake()
    {
        AgentBehaviour = GetComponent<AgentBehaviour>();
    }

    private void Start()
    {
        AgentBehaviour.SetGoal<WanderGoal>(false);
    }

    private void OnEnable()
    {
        AgentBehaviour.EndAction();
        AgentBehaviour.Events.OnNoActionFound += OnNoActionFound;

        LOSSensor.OnTargetEnter += LOSSensorSensorOnTargetEnter;
        LOSSensor.OnTargetExit += LOSSensorOnTargetExit;
    }

    private void OnNoActionFound(IGoalBase goal)
    {
        AgentBehaviour.SetGoal<WanderGoal>(true);
    }

    private void OnDisable()
    {
        LOSSensor.OnTargetEnter -= LOSSensorSensorOnTargetEnter;
        LOSSensor.OnTargetExit -= LOSSensorOnTargetExit;
    }

    private void LOSSensorSensorOnTargetEnter(Transform player)
    {
        AgentBehaviour.SetGoal<KillPlayerGoal>(true);
    }

    private void LOSSensorOnTargetExit(Transform player)
    {
        AgentBehaviour.SetGoal<WanderGoal>(true);
    }

}
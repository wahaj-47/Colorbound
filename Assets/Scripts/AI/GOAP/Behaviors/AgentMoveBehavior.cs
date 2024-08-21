using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterMovement), typeof(NavMeshAgent), typeof(AgentBehaviour))]
public class AgentMoveBehavior : MonoBehaviour
{
    private CharacterMovement CharacterMovement;
    private PlayerCharacterInputs PlayerCharacterInputs;
    private NavMeshAgent NavMeshAgent;
    private AgentBehaviour AgentBehaviour;

    private ITarget CurrentTarget;
    [SerializeField] private float MinMoveDistance = 0.25f;
    private Vector3 LastPosition;

    private void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        AgentBehaviour = GetComponent<AgentBehaviour>();
    }

    private void OnEnable()
    {
        AgentBehaviour.Events.OnTargetInRange += EventsOnTargetInRange;
        AgentBehaviour.Events.OnTargetChanged += EventsOnTargetChanged;
    }

    private void OnDisable()
    {
        AgentBehaviour.Events.OnTargetInRange -= EventsOnTargetInRange;
        AgentBehaviour.Events.OnTargetChanged -= EventsOnTargetChanged;
    }

    private void Start()
    {
        PlayerCharacterInputs = new PlayerCharacterInputs();
    }

    private void Update()
    {
        HandleCharacterInputs();

        if (CurrentTarget == null) return;

        if (MinMoveDistance <= Vector3.Distance(CurrentTarget.Position, LastPosition))
        {
            LastPosition = CurrentTarget.Position;
            NavMeshAgent.destination = CurrentTarget.Position;
        }

    }

    private void HandleCharacterInputs()
    {
        Vector3 moveDirection = NavMeshAgent.velocity.magnitude < 1 ? Vector3.zero : NavMeshAgent.velocity;

        // Build the CharacterInputs struct
        PlayerCharacterInputs.MoveAxisForward = moveDirection.z;
        PlayerCharacterInputs.MoveAxisRight = moveDirection.x;

        // Apply inputs to character
        CharacterMovement.SetInputs(ref PlayerCharacterInputs);
    }

    private void EventsOnTargetInRange(ITarget target)
    {
        CurrentTarget = target;
    }

    private void EventsOnTargetChanged(ITarget target, bool inRange)
    {
        CurrentTarget = target;
        LastPosition = CurrentTarget.Position;
        NavMeshAgent.destination = CurrentTarget.Position;
    }

}
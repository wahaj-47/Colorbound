using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIState
{
    // Maybe expose it as an attribute or via "StateMachine"
    private readonly float acceptanceRadius = 1f;
    private readonly float idleTime = 3f;
    private bool idling = false;
    private Vector3 patrolPoint;

    public AIPatrolState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log($"{StateMachine.name}: Patrol State");

        base.EnterState();

        Patrol();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(HasReachedDestination(acceptanceRadius) && !idling) 
        {
            StateMachine.StartCoroutine(Idle());
        }

        if(targetAcquired)
        {
            StateMachine.SwitchState((StateMachine as AIStateMachine).CombatState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        targetAcquired = false;
    }

    private void Patrol(float radius = 3)
    {
        idling = false;
        patrolPoint = RandomReachablePointInRadius(radius, (StateMachine as AIStateMachine).spawnPosition);
        agent.destination = patrolPoint;
    }

    private IEnumerator Idle()
    {
        idling = true;
        
        yield return new WaitForSeconds(idleTime);

        Patrol(
            (StateMachine as AIStateMachine)
            .GetAttributeValue((StateMachine as AIStateMachine).patrolRadius)
            .CurrentValue
        );
    }
}

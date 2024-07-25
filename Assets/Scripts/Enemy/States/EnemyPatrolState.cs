using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyState
{
    // Maybe expose it as an attribute or via "StateMachine"
    private float acceptanceRadius = 0.05f;
    private float idleTime = 3f;
    private bool idling = false;
    private Vector3 patrolPoint;

    public EnemyPatrolState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Patrol();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(HasReachedDestination() && !idling) 
        {
            StateMachine.StartCoroutine(Idle());
        }
    }

    private void Patrol(float radius = 3)
    {
        idling = false;
        patrolPoint = RandomReachablePointInRadius(radius);
        agent.destination = patrolPoint;
    }

    private bool HasReachedDestination()
    {
        return agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < acceptanceRadius;
    }

    private Vector3 RandomReachablePointInRadius(float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += (StateMachine as EnemyStateMachine).spawnPosition;
        NavMeshHit hit;
        Vector3 reachablePosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas)) {
            reachablePosition = hit.position;            
        }
        return reachablePosition;
    }

    private IEnumerator Idle()
    {
        idling = true;
        
        yield return new WaitForSeconds(idleTime);

        Patrol(
            (StateMachine as EnemyStateMachine)
            .GetAttributeValue((StateMachine as EnemyStateMachine).patrolRadius)
            .CurrentValue
        );
    }
}

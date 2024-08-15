using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIState : BaseState
{
    protected NavMeshAgent agent;
    protected bool dangerDetected = false;
    protected bool targetAcquired = false;
    public AIState(BaseStateMachine stateMachine) : base(stateMachine)
    {
        agent = StateMachine.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        base.EnterState();

        StateMachine.StartCoroutine(FindTargetsWithDelay());
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();

        StateMachine.StopAllCoroutines();
    }

    private IEnumerator FindTargetsWithDelay(float delay = 0f)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();

            delay = (StateMachine as AIStateMachine)
                    .GetAttributeValue((StateMachine as AIStateMachine).scanInterval)
                    .CurrentValue;
        }
    }

    private void FindVisibleTargets()
    {
        bool targetInRange = false;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(
            StateMachine.transform.position, 
            (StateMachine as AIStateMachine).GetAttributeValue((StateMachine as AIStateMachine).sightRange).CurrentValue, 
            (StateMachine as AIStateMachine).whatIsTarget
        );

        for(int i=0; i < targetsInViewRadius.Length; i++)
        {
            float smallestDist = Mathf.Infinity;
            Collider target = targetsInViewRadius[i];
            Vector3 dirToTarget = target.transform.position - StateMachine.transform.position;

            if(
                Vector3.Angle(StateMachine.transform.forward, dirToTarget.normalized) 
                < (StateMachine as AIStateMachine).GetAttributeValue((StateMachine as AIStateMachine).viewAngle).CurrentValue/2
            )
            {
                if (!Physics.Raycast(StateMachine.transform.position, dirToTarget, out RaycastHit hit, (StateMachine as AIStateMachine).whatIsObstacle))
                {
                    if (hit.distance < smallestDist)
                    {
                        targetInRange = true;
                        
                        if(target.TryGetComponent<AbilityManager>(out var abilityManager))
                        {
                            abilityManager.attackEvent.RemoveListener(Danger);
                            abilityManager.attackEvent.AddListener(Danger);
                        }

                        (StateMachine as AIStateMachine).target = target;
                    }
                }
            }
        }

        targetAcquired = targetInRange;
    }

    public bool HasReachedDestination(float acceptanceRadius)
    {
        return agent.remainingDistance != Mathf.Infinity 
        && agent.pathStatus == NavMeshPathStatus.PathComplete 
        && agent.remainingDistance < (acceptanceRadius + 1.25 * agent.radius);
    }

    public Vector3 RandomReachablePointInRadius(float radius, Vector3 position) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += position;
        NavMeshHit hit;
        Vector3 reachablePosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas)) {
            reachablePosition = hit.position;            
        }
        return reachablePosition;
    }

    private void Danger()
    {
        dangerDetected = true;
    }
}

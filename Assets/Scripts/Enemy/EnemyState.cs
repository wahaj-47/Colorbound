using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : BaseState
{
    protected NavMeshAgent agent;
    protected Transform[] players = new Transform[3];
    protected Transform closestTarget; 
    
    public EnemyState(BaseStateMachine stateMachine) : base(stateMachine)
    {
        agent = StateMachine.GetComponent<NavMeshAgent>();

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        for(int i=0; i<players.Length; i++)
        {
            players[i] = targets[i].transform;
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        StateMachine.StartCoroutine(
            FindTargetsWithDelay(
                (StateMachine as EnemyStateMachine)
                .GetAttributeValue((StateMachine as EnemyStateMachine).scanInterval)
                .CurrentValue)
        );
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();

            delay = (StateMachine as EnemyStateMachine)
                    .GetAttributeValue((StateMachine as EnemyStateMachine).scanInterval)
                    .CurrentValue;
        }
    }

    private void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(
            StateMachine.transform.position, 
            (StateMachine as EnemyStateMachine).GetAttributeValue((StateMachine as EnemyStateMachine).sightRange).CurrentValue, 
            (StateMachine as EnemyStateMachine).whatIsPlayer);

        for(int i=0; i < targetsInViewRadius.Length; i++)
        {
            float smallestDist = Mathf.Infinity;
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = target.position - StateMachine.transform.position;

            if(
                Vector3.Angle(StateMachine.transform.forward, dirToTarget.normalized) 
                < (StateMachine as EnemyStateMachine).GetAttributeValue((StateMachine as EnemyStateMachine).viewAngle).CurrentValue/2)
            {
                RaycastHit hit;
                if(!Physics.Raycast(StateMachine.transform.position, dirToTarget, out hit, (StateMachine as EnemyStateMachine).whatIsObstacle))
                {
                    if(hit.distance < smallestDist)
                    {
                        closestTarget = target;
                    }
                }
            }
        }
    }
}

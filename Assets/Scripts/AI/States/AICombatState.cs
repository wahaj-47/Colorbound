using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICombatState : AIState
{
    private bool didAttack = false;
    public AICombatState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log($"{StateMachine.name}: Combat state");
        base.EnterState();

        (StateMachine as AIStateMachine).Controller.Hunt();

        targetAcquired = true;
        agent.destination = (StateMachine as AIStateMachine).target.position;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(targetAcquired)
        {
            agent.destination = (StateMachine as AIStateMachine).target.position;

            if(dangerDetected)
            {
                if(HasReachedDestination((StateMachine as AIStateMachine).abilityManagerComponent.abilityOne.Range * 1.2f))
                {
                    agent.velocity *= -1;
                    // (StateMachine as AIStateMachine).Controller.Jump();
                    (StateMachine as AIStateMachine).Controller.Dash();
                }
                
                dangerDetected = false;
            }

            if(!didAttack)
            {
                if(HasReachedDestination((StateMachine as AIStateMachine).abilityManagerComponent.abilityOne.Range))
                {
                    StateMachine.StartCoroutine(AbilityOne());
                }
            }
        }
        else
        {
            StateMachine.SwitchState((StateMachine as AIStateMachine).PatrolState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        targetAcquired = false;
        didAttack = false;
    }

    private IEnumerator AbilityOne()
    {
        // Performed melee
        didAttack = true;
        (StateMachine as AIStateMachine).Controller.AbilityOne();

        // Adding an artificial delay for 0.2 seconds before resetting the melee attack
        yield return new WaitForSeconds(0.2f);
        didAttack = false;
    }

}

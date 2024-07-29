using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIController), typeof(NavMeshAgent), typeof(AIStateMachine))]
public class AIControllerManager : MonoBehaviour
{
    private AIController _AIController;
    private AIStateMachine _AIStateMachine;
    private NavMeshAgent _NavmeshAgent;

    private void Awake()
    {
        _AIController = GetComponent<AIController>();
        _AIStateMachine = GetComponent<AIStateMachine>();
        _NavmeshAgent = GetComponent<NavMeshAgent>();
    }
    public void HandlePosession()
    {
        _AIController.enabled = false;
        _AIStateMachine.enabled = false;
        _NavmeshAgent.enabled = false;
    }

    public void HandleUnPosession()
    {
        _AIController.enabled = true;
        _AIStateMachine.enabled = true;
        _NavmeshAgent.enabled = true;
    }
}

using CrashKonijn.Goap.Behaviours;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AgentBrain), typeof(AgentBehaviour), typeof(AgentMoveBehavior))]
public class AIHandler : MonoBehaviour
{
    private AgentBrain AgentBrain;
    private AgentBehaviour AgentBehaviour;
    private AgentMoveBehavior AgentMoveBehavior;

    private void Awake()
    {
        AgentBrain = GetComponent<AgentBrain>();
        AgentBehaviour = GetComponent<AgentBehaviour>();
        AgentMoveBehavior = GetComponent<AgentMoveBehavior>();
    }

    public void EnableAI()
    {
        AgentBrain.enabled = true;
        AgentBehaviour.enabled = true;
        AgentMoveBehavior.enabled = true;
    }

    public void DisableAI()
    {
        AgentBrain.enabled = false;
        AgentBehaviour.enabled = false;
        AgentMoveBehavior.enabled = false;
    }

}

using CrashKonijn.Goap.Behaviours;
using UnityEngine;

[RequireComponent(typeof(AgentBehaviour))]
public class GoapSetBinder : MonoBehaviour
{
    [SerializeField] private GoapRunnerBehaviour GoapRunner;

    private void Awake()
    {
        AgentBehaviour agent = GetComponent<AgentBehaviour>();
        agent.GoapSet = GoapRunner.GetGoapSet("EnemySet");
    }
}

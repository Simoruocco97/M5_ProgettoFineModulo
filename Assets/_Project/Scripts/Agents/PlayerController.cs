using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private AgentMover agentMover;
    [SerializeField] private NavMeshAgent agent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (agentMover == null)
            agentMover = GetComponent<AgentMover>();
    }

    public void EnablePlayer(bool enable)
    {
        if (agentMover != null)
            agentMover.enabled = enable;

        if (agent != null)
            agent.isStopped = !enable;

        if (!enable)
            agent.velocity = Vector3.zero;
    }
}
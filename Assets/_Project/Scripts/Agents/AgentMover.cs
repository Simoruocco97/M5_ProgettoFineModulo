using UnityEngine;
using UnityEngine.AI;

public class AgentMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Camera cameraMain;
    [SerializeField] private GameObject clickIndicatorPref;
    private bool isMoving = false;

    private void Awake()
    {
        if (cameraMain == null)
            cameraMain = Camera.main;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int layerMaskToIgnore = ~LayerMask.GetMask("TriggerCollider"); //ignoro il layer selezionato (problema con i trigger collider)
            Ray mouseRay = cameraMain.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, layerMaskToIgnore))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))   //garantisce che la destinazione sia valida sulla NavMesh
                {
                    agent.SetDestination(navHit.position);

                    if (clickIndicatorPref != null)
                    {
                        GameObject indicator = Instantiate(clickIndicatorPref, navHit.position + Vector3.up * 0.05f, Quaternion.Euler(90, 0, 0));
                    }
                }
            }
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        bool isCurrentlyMoving = agent.hasPath && agent.velocity.sqrMagnitude > 0.01f;  //se l'agent ha un path e non è fermo

        if (isCurrentlyMoving != isMoving)  //allora, al cambio di stato aggiorno
        {
            isMoving = isCurrentlyMoving;

            if (PlayerAnimatorManager.Instance != null)
                PlayerAnimatorManager.Instance.MovementAnimation(isMoving);
        }
    }
}
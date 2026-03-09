using UnityEngine;

public class EnemyConeVision : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField] private Transform castPoint;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacles;

    [Header("Cone Settings")]
    [SerializeField] private float viewDistance = 20f;
    [SerializeField] private float coneAngle = 45f;

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (castPoint == null)
            Debug.LogWarning($"{gameObject.name} non ha castPoint settato!");

        if (obstacles == 0)
        {
            Debug.LogWarning($"{gameObject.name} non ha layerMask settato!");
        }
    }

    public bool PlayerInConeVision()
    {
        if (player == null)
            return false;

        Vector3 distance = player.position - transform.position;
        float distanceMag = distance.magnitude;

        //player a distanza maggiore della viewDistance
        if (distanceMag > viewDistance)
            return false;

        distance = distance.normalized;

        //player fuori dall'angolo del cono di visione del nemico
        if (Vector3.Dot(transform.forward, distance) < Mathf.Cos(coneAngle * Mathf.Deg2Rad))
            return false;

        //se ci sono ostacoli tra enemy e player
        if (Physics.Raycast(castPoint.position, distance, viewDistance, obstacles))
            return false;

        return true;
    }

    //test, da eliminare
    private void OnDrawGizmos()
    {
        if (castPoint == null)
            return;

        Gizmos.color = Color.yellow;

        //forward del castPoint sul piano XZ
        Vector3 forward = Vector3.ProjectOnPlane(castPoint.forward, Vector3.up).normalized;

        //castPoint del centro
        Gizmos.DrawRay(castPoint.position, forward * viewDistance);

        //calcolo angoli ai bordi
        float halfAngle = coneAngle / 2f;
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * forward;
        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * forward;

        //proietto i bordi degli angoli
        Gizmos.DrawRay(castPoint.position, rightDir * viewDistance);
        Gizmos.DrawRay(castPoint.position, leftDir * viewDistance);
    }
}
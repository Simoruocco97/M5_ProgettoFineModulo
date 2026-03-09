using UnityEngine;

public class EnemyPatrol : Enemy
{
    [Header("Checkpoints Infos")]
    [SerializeField] private Transform[] checkpoints;
    [SerializeField] private float offset = 0.5f;
    [SerializeField] private float pauseDuration = 1.5f;
    private float pauseTime = 0f;
    private bool isPaused = false;
    private int index;

    protected override void PatrolBehaviour()
    {
        if (checkpoints.Length == 0) return;

        if (isPaused)
        {
            pauseTime -= Time.deltaTime;

            if (pauseTime <= 0f)
                isPaused = false;
            else
                return;
        }

        Vector3 targetPos = checkpoints[index].position;

        if (enemy.destination != targetPos)
            enemy.SetDestination(targetPos);

        if (!enemy.pathPending && enemy.remainingDistance <= enemy.stoppingDistance + offset)
        {
            isPaused = true;
            pauseTime = pauseDuration;

            index++;
            if (index >= checkpoints.Length)
                index = 0;
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject player;
    [SerializeField] protected NavMeshAgent enemy;
    [SerializeField] private EnemyConeVision enemyVision;

    [Header("CheckPoint Manager")]
    [SerializeField] private Transform patrolStartPoint;

    [Header("State Infos")]
    private State currentState;

    [Header("Attack Infos")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private int damage = 35;
    private float lastHitTime = 0;

    [Header("Search Infos")]
    [SerializeField] private float searchDuration = 6f;
    [SerializeField] private float rotationSwap = 2f;
    [SerializeField] private float angleSearch = 45f;
    private Vector3 lastKnownPos;
    private float searchTimer;

    [Header("Animation")]
    [SerializeField] private EnemyAnimatorManager animator;

    [Header("Audio")]
    [SerializeField] private float soundCooldown = 5f;
    private float lastSoundTime;

    [Header("Optimizzation Settings")]
    [SerializeField] private float updatePathInterval = 0.5f;
    [SerializeField] private float threshold = 0.5f;
    private float lastPathUpdateTime;

    private enum State
    {
        Patrol,
        Chase,
        Attack,
        Search,
        ReturnToPatrol
    }

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (enemy == null)
            enemy = GetComponent<NavMeshAgent>();

        if (patrolStartPoint == null)
            patrolStartPoint = enemy.transform;

        if (enemyVision == null)
            enemyVision = GetComponent<EnemyConeVision>();

        if (animator  == null)
            animator = GetComponent<EnemyAnimatorManager>();

        currentState = State.Patrol;
    }

    private void Update()
    {
        if (animator != null)
        {
            bool isMoving = enemy.hasPath && enemy.remainingDistance > enemy.stoppingDistance;
            animator.MovementAnimation(isMoving);
        }

        switch (currentState)
        {
            case State.Patrol:
                PatrolFunc();
                break;

            case State.Chase:
                ChaseFunc();
                break;

            case State.Attack:
                AttackFunc();
                break;

            case State.Search:
                SearchFunc();
                break;

            case State.ReturnToPatrol:
                ReturnToPatrolFunc();
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        switch (newState)
        {
            case State.Search:
                searchTimer = 0;
                break;

            case State.Chase:
                enemy.isStopped = false;    //fix per cui ogni tanto l'enemy si bloccava sul posto
                enemy.updateRotation = true;
                enemy.SetDestination(player.transform.position);

                if (Time.time >= lastSoundTime + soundCooldown)
                {
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlaySFXSound("playerSpotted");
                        lastSoundTime = Time.time;
                    }
                }
                break;

            case State.Attack:
                enemy.isStopped = true;
                break;

            default:
                enemy.isStopped = false;
                enemy.updateRotation = true;
                break;
        }
    }

    private void PatrolFunc()
    {
        if (enemyVision != null && enemyVision.PlayerInConeVision())
        {
            ChangeState(State.Chase);
            return;
        }
        PatrolBehaviour();
    }

    private void ChaseFunc()
    {
        if (enemyVision != null && !enemyVision.PlayerInConeVision())
        {
            lastKnownPos = player.transform.position;
            ChangeState(State.Search);
            return;
        }

        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            lastKnownPos = player.transform.position;
            enemy.SetDestination(lastKnownPos);
            lastPathUpdateTime = Time.time;
        }

        if (CanPerformAttack())
            ChangeState(State.Attack);
    }

    private void AttackFunc()
    {
        if (enemyVision != null && !enemyVision.PlayerInConeVision())
        {
            ChangeState(State.Chase);
            return;
        }

        if (!CanPerformAttack())
        {
            ChangeState(State.Chase);
            return;
        }

        if (Time.time >= lastHitTime + attackCooldown)
        {
            if (animator != null)
                animator.AttackAnimation();

            var playerLife = player.GetComponent<LifeController>();
            if (playerLife != null)
            {
                playerLife.TakeDamage(damage);
                lastHitTime = Time.time;

                if (playerLife.DeathStatus())
                    ChangeState(State.ReturnToPatrol);
            }
        }
    }

    private void SearchFunc()
    {
        if (enemy.remainingDistance > threshold)                        //se non ancora arrivato
        {
            if (Time.time - lastPathUpdateTime > updatePathInterval)    //setta la destinazione ogni tot secondi
            {
                enemy.SetDestination(lastKnownPos);
                lastPathUpdateTime = Time.time;
            }
            return;
        }

        enemy.isStopped = true;
        searchTimer += Time.deltaTime;

        float oscillazione = Mathf.Sin(Time.time * rotationSwap) * angleSearch; //Mathf.Sin calcola
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + (oscillazione * Time.deltaTime), 0);

        if (enemyVision.PlayerInConeVision())
        {
            ChangeState(State.Chase);
        }

        if (searchTimer >= searchDuration)
        {
            enemy.isStopped = false;
            ChangeState(State.ReturnToPatrol);
        }
    }

    private void ReturnToPatrolFunc()
    {
        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            enemy.SetDestination(patrolStartPoint.transform.position);
            lastPathUpdateTime = Time.time;
        }

        if (HasReachedCheckpoint())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, patrolStartPoint.rotation, Time.deltaTime * 5f); //forzo la rotazione per allinearmi con lo startPoint

            if (Quaternion.Angle(transform.rotation, patrolStartPoint.rotation) < threshold)
            {
                ChangeState(State.Patrol);
            }
        }
    }

    protected virtual void PatrolBehaviour() { }

    private bool CanPerformAttack()
    {
        return Vector3.Distance(player.transform.position, enemy.transform.position) < attackRange;
    }

    private bool HasReachedCheckpoint()
    {
        return Vector3.Distance(enemy.transform.position, patrolStartPoint.position) < threshold;
    }
}
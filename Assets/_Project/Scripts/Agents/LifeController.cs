using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int minHp = 0;
    [SerializeField] private bool fullHpOnStart = false;
    [SerializeField] private float damageAnimationCooldown = 1f;
    private readonly int hpOnStart = 50;
    private int currentHp;
    private bool isDead = false;
    private float lastDamage = 0f;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent<int, int> onHpChange;
    [SerializeField] private UnityEvent onDefeat;

    private void Awake()
    {
        if (fullHpOnStart)
            SetHp(maxHp);
        else
            SetHp(hpOnStart);
    }

    private int GetHp()
    {
        return currentHp;
    }

    private void SetHp(int hp)
    {
        currentHp = Mathf.Clamp(hp, minHp, maxHp);
        onHpChange.Invoke(currentHp, maxHp);

        Debug.Log($"{gameObject.name} ha {GetHp()} Hp");
    }

    public void AddHp(int heal)
    {
        if (heal < 0) heal = 0;
        SetHp(currentHp + heal);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        SetHp(currentHp - damage);

        if (GetHp() <= minHp)
        {
            OnDeath();
            return;
        }

        if (!isDead)
        {
            if (Time.time >= lastDamage + damageAnimationCooldown)
            {
                if (PlayerAnimatorManager.Instance != null)
                    PlayerAnimatorManager.Instance.PlayDamageAnimation();

                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFXSound("damage");

                lastDamage = Time.time;
            }
        }
    }

    private void OnDeath()
    {
        isDead = true;

        if (PlayerAnimatorManager.Instance != null && PlayerController.Instance != null)
        {
            PlayerController.Instance.EnablePlayer(false);
            PlayerAnimatorManager.Instance.DeathAnimation();

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFXSound("deathSound");

            var agent = GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = false;
        }

        Invoke(nameof(OnDefeat), 5);
    }

    private void OnDefeat()
    {
        onDefeat.Invoke();
    }

    public bool DeathStatus()
    {
        return isDead;
    }
}
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    public static PlayerAnimatorManager Instance { get; private set; }

    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void MovementAnimation(bool isMoving)
    {
        if (animator == null)
            return;

        animator.SetBool("isMoving", isMoving);
    }

    public void PlayDamageAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Damage");
    }

    public void DeathAnimation()
    {
        if (animator != null)
            animator.SetTrigger("isDead");
    }
}
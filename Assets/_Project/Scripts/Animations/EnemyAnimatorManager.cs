using UnityEngine;

public class EnemyAnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void MovementAnimation(bool isMoving)
    {
        if (animator == null)
            return;

        animator.SetBool("isMoving", isMoving);
    }

    public void AttackAnimation()
    {
        if (animator == null) return;
        animator.SetTrigger("Attack");
    }
}
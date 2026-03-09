using UnityEngine;

public class LeverAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void OnLeverDown()
    {
        if (animator != null)
            animator.SetTrigger("leverDown");
    }
}
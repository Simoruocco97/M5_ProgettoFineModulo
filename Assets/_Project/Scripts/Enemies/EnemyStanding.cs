using UnityEngine;

public class EnemyStanding : Enemy
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationInterval = 5f;
    [SerializeField] private float rotationAngle = 180f;
    [SerializeField] private float rotationSpeedStanding = 90f;

    private float rotationTimer = 0f;
    private Quaternion targetRotation;

    protected override void PatrolBehaviour()
    {
        rotationTimer += Time.deltaTime;
        if (rotationTimer > rotationInterval)
        {
            rotationTimer = 0f;
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAngle, 0);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeedStanding * Time.deltaTime);
    }
}
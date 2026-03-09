using Cinemachine;
using System.Collections;
using UnityEngine;

public class LeverInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject door;
    [SerializeField] private CinemachineVirtualCamera cinematicCamera;
    [SerializeField] private int destroyTimer = 3;
    [SerializeField] private LeverAnimation animator;
    private readonly float cameraWaitOffset = 0.5f;
    private bool isActivated = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<LeverAnimation>();
    }

    public void Interact()
    {
        if (isActivated)
            return;

        isActivated = true;

        if (animator != null)
            animator.OnLeverDown();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFXSound("lever");

        StartCoroutine(PlayCameraMovement());
    }

    public string InteractionText()
    {
        return isActivated ? null : "Premi F per attivare la leva";
    }

    private IEnumerator PlayCameraMovement()
    {
        PlayerController.Instance.EnablePlayer(false);

        if (cinematicCamera != null)
            cinematicCamera.Priority = 20;

        yield return new WaitForSeconds(destroyTimer * cameraWaitOffset);

        Destroy(door);

        yield return new WaitForSeconds(destroyTimer * cameraWaitOffset);

        if (cinematicCamera != null)
            cinematicCamera.Priority = 5;

        PlayerController.Instance.EnablePlayer(true);
    }
}
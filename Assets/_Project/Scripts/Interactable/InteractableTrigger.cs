using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField] private Collider triggerZone;
    private IInteractable interactable;

    private void Awake()
    {
        if (triggerZone == null)
            triggerZone = GetComponent<Collider>();

        if (triggerZone != null)
            triggerZone.isTrigger = true;

        interactable = GetComponentInParent<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other))
            return;

        PlayerInteraction player = other.GetComponent<PlayerInteraction>();
        if (player != null)
            player.SetInteractable(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other))
            return;

        PlayerInteraction player = other.GetComponent<PlayerInteraction>();
        if (player != null)
            player.RemoveInteractable();
    }

    private bool IsPlayer(Collider other) => other.CompareTag("Player");
}
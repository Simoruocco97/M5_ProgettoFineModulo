using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactionCanvas;
    [SerializeField] private TextMeshProUGUI interactionText;
    private KeyCode interactionKeyCode = KeyCode.F;
    private IInteractable interactable;
    private bool isUIVisible = false;

    private void Awake()
    {
        if (interactionCanvas == null)
            Debug.LogWarning($"interactionCanvas non assegnata su {gameObject.name}");

        if (interactionText == null)
            Debug.LogWarning($"interactionText non assegnata su {gameObject.name}");

        if (interactionCanvas != null)
            interactionCanvas.SetActive(false);
    }

    private void Update()
    {
        if (interactable == null)
        {
            if (isUIVisible && interactionCanvas != null)
            {
                interactionCanvas.SetActive(false);
                isUIVisible = false;
            }
            return;
        }

        string text = interactable.InteractionText();

        if (!string.IsNullOrEmpty(text))
        {
            if (!isUIVisible && interactionCanvas != null)
            {
                interactionCanvas.SetActive(true);
                isUIVisible = true;
            }

            if (interactionText != null)
                interactionText.text = text;
        }
        else
        {
            if (isUIVisible && interactionCanvas != null)
            {
                interactionCanvas.SetActive(false);
                isUIVisible = false;
            }
        }

        if (Input.GetKeyDown(interactionKeyCode))
            interactable.Interact();
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
    }

    public void RemoveInteractable()
    {
        interactable = null;
    }
}
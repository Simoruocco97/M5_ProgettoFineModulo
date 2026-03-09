using System.Collections;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private float dialogueTime = 5f;
    private bool isDialogueOpen = false;

    private void Awake()
    {
        if (dialogue != null)
            dialogue.SetActive(false);
    }

    public void Interact()
    {
        if (dialogue == null || isDialogueOpen)
            return;

        StartCoroutine(Dialogue());
    }

    public string InteractionText()
    {
        return "Premi F per interagire";
    }

    private IEnumerator Dialogue()
    {
        dialogue.SetActive(true);
        isDialogueOpen = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFXSound("dialogue");

        yield return new WaitForSeconds(dialogueTime);

        dialogue.SetActive(false);
        isDialogueOpen = false;
    }
}
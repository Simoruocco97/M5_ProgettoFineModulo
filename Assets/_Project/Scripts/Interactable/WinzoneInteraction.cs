using UnityEngine;

public class WinzoneInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private UI_Win UI_Win;

    private void Awake()
    {
        if (UI_Win == null)
            UI_Win = FindAnyObjectByType<UI_Win>();
    }

    public void Interact()
    {
        if (UI_Win == null)
        {
            Debug.LogWarning("UI_Win non trovato nella scena!");
            return;
        }

        PlayerController.Instance.EnablePlayer(false);
        UI_Win.ShowWinCanvas();
    }

    public string InteractionText()
    {
        return "Premi F per fuggire dal condotto";
    }
}
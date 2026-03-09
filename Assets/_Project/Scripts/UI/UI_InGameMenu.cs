using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UI_InGameMenu : MonoBehaviour
{
    [SerializeField] private string mainMenu = "MainMenu";
    [SerializeField] protected CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        HideMenu();
    }

    protected void ShowMenu()
    {
        if (canvasGroup == null)
            return;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        StopTime();
    }

    protected void HideMenu()
    {
        if (canvasGroup == null) 
            return;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        ResumeTime();
    }

    public void ReturnToMainMenu()
    {
        ResumeTime();
        SceneManager.LoadScene(mainMenu);
    }

    protected void ResumeTime()
    {
        Time.timeScale = 1.0f;
    }

    protected void StopTime()
    {
        Time.timeScale = 0f;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_SceneManager : MonoBehaviour

{
    [Header("Scene Swap")]
    [SerializeField] private string mainLevel = "MainLevel";
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject tutorialTab;
    private CanvasGroup canvasGroup;

    public void OnStartPress()
    {
        SceneManager.LoadScene(mainLevel);
    }

    public void OnOptionPress()
    {
        if (tutorialTab != null)
            tutorialTab.SetActive(true);

        if (mainMenu != null && canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.5f;
        }
    }

    public void OnOptionClose()
    {
        if (tutorialTab != null)
            tutorialTab.SetActive(false);

        if (mainMenu != null && canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

    public void OnExitPress()
    {
        Debug.Log("Sei uscito dal gioco");
        Application.Quit();
    }

    private void Awake()
    {
        if (mainMenu == null)
            Debug.LogWarning("mainCanvas non assegnata");

        if (tutorialTab == null)
            Debug.LogWarning("OptionsTab non assegnata");

        if (mainMenu != null) 
        {
            mainMenu.SetActive(true);
            canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        if (tutorialTab != null)
            tutorialTab.SetActive(false);
    }   
}
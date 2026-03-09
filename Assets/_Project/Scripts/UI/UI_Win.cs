using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Win : UI_InGameMenu
{
    [SerializeField] private GameObject winCanvas;

    public void ShowWinCanvas()
    {
        if (winCanvas == null)
            return;

        ShowMenu();
        PlayerController.Instance.EnablePlayer(false);
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
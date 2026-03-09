using UnityEngine;

public class UI_Pause : UI_InGameMenu
{
    private bool menuOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void Resume() => ResumeGame();

    private void TogglePause()
    {
        if (menuOpen)
            ResumeGame();
        else
            PauseGame();
    }

    private void ResumeGame()
    {
        menuOpen = false;
        HideMenu();

        PlayerController.Instance.EnablePlayer(true);
    }

    private void PauseGame()
    {
        menuOpen = true;
        ShowMenu();

        PlayerController.Instance.EnablePlayer(false);
    }
}
using UnityEngine.SceneManagement;

public class UI_GameOver : UI_InGameMenu
{
    public void RestartLevel()
    {
        ResumeTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowGameOver()
    {
        ShowMenu();
    }
}
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Dừng mọi thứ liên quan đến thời gian game
        AudioListener.pause = true; // Dừng toàn bộ âm thanh đang phát
        isPaused = true;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Khôi phục hoạt động
        AudioListener.pause = false; // Mở lại âm thanh
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void ReturnToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneName);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Application.Quit();
    }

}

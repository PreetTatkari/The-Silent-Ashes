using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign this in the Inspector

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ExitGame();
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    void ExitGame()
    {
        Debug.Log("Exiting Game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For exiting play mode in the editor
#else
        Application.Quit(); // For exiting the built application
#endif
    }
}

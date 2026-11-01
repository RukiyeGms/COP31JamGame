
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    private void Awake()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (IntroUI.Instance != null && IntroUI.Instance.IsShowing)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameManager.IsPaused)
        {
            Resume();
        }
        else if (GameManager.IsGamePlaying)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (!GameManager.IsGamePlaying)
            return;

        GameManager.instance.PauseGame();
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        if (!GameManager.IsPaused)
            return;

        pausePanel.SetActive(false);
        GameManager.instance.ResumeGame();
    }

    public void GoToMainMenu()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ReturnToMainMenu();
        }
    }
}


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManager;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;
    public Button pauseButton;

    private void Start()
    {
        // Button listeners
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }
    }

    private void OnResumeButtonClicked()
    {
        gameManager.ResumeGame();
    }

    private void OnRestartButtonClicked()
    {
        gameManager.StartupCanvas();
    }

    private void OnPauseButtonClicked()
    {
        gameManager.PauseGame();
    }

    private void OnQuitButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Reference to the pause canvas GameObject
    public GameObject pauseCanvas;

    // Flag to track if the game is paused
    private bool isPaused = false;

    private OptionsMenu optionsMenu;

    private void Start()
    {
        optionsMenu = FindObjectOfType<OptionsMenu>();
    }


    private void Update()
    {
        // Check if the player presses the Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the game is paused, call the Resume() method
            if (isPaused)
            {
                Resume();
            }
            // If the game is not paused, call the Pause() method
            else
            {
                Pause();
            }
        }
    }

    // Method to pause the game
    public void Pause()
    {
        // Pause the game time
        Time.timeScale = 0f;

        // Set the isPaused flag to true
        isPaused = true;

        // Activate the pauseCanvas
        pauseCanvas.SetActive(true);
    }

    // Method to resume the game
    public void Resume()
    {
        // Resume the game time
        Time.timeScale = 1f;

        // Set the isPaused flag to false
        isPaused = false;

        // Deactivate the pauseCanvas
        pauseCanvas.SetActive(false);
    }

    // Method to restart the current scene
    public void Restart()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Resume the game time
        Time.timeScale = 1f;
    }

    // Loads the main menu
    public void MainMenu()
    {
        // Loads the main menu
        SceneManager.LoadScene("MainMenu");

        // Resume the game time
        Time.timeScale = 1f;
    }

    // loads the options menu
    public void Options()
    {
        if (optionsMenu != null)
        {
            // Store the current scene index 
            optionsMenu.previousSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Loads the Options
            SceneManager.LoadScene("Options");
        }
        else
        {
            SceneManager.LoadScene("Options");
        }

        // Resume the game time
        Time.timeScale = 1f;
    }
}
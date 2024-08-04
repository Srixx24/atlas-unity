using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public void MainMenu()
    {
        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void Next()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Check if the current scene is the last level
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            // If it's the last level, load the MainMenu scene
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // Load the next level
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
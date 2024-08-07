using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    // Button click logic
    public Button playButton;
    public Button quitButton;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }
    // Start maze game
    public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Quit game
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}

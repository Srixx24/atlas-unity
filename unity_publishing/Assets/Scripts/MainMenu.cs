using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Button click logic
    public Button playButton;
    public Button quitButton;

    // Color Blind settings
    public Material trapMat;
    public Material goalMat;
    public Toggle colorblindMode;
    void Start()
    {
        playButton.onClick.AddListener(PlayMaze);
        quitButton.onClick.AddListener(QuitMaze);
    }
    // Start maze game
    public void PlayMaze()
    {
        SceneManager.LoadScene("maze");

        if (colorblindMode.isOn)
        {
            // Change the trap and goal material colors
            trapMat.color = new Color32(255, 112, 0, 255);
            goalMat.color = Color.blue;
        }
        else
        {
            // Original colors for materials
            trapMat.color = Color.red;
            goalMat.color = Color.green;
        }
    }

    // Quit game
    public void QuitMaze()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
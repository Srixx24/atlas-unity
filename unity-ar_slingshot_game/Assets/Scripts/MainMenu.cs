using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject infoCanvas;
    public GameObject mainMenuCanvas;
    public Button playButton;
    public Button infoButton;
    public Button exitButton;

    void Start()
    {
        infoCanvas.SetActive(false);

        playButton.onClick.AddListener(OnPlayButton);
        infoButton.onClick.AddListener(OnInfoButton);
        exitButton.onClick.AddListener(OnExitButton);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitButton()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnInfoButton()
    {
        // Switch to Info Canvas
        mainMenuCanvas.SetActive(false);
        infoCanvas.SetActive(true);
    }
}
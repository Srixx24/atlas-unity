using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button Level01;
    public Button Level02;
    public Button Level03;
    public Button exitButton;
    public Button optionsButton;

    private void Start()
    {
        Level01.onClick.AddListener(delegate { LoadLevel(3); });
		Level02.onClick.AddListener(delegate { LoadLevel(4); });
        Level03.onClick.AddListener(delegate { LoadLevel(5); });
        exitButton.onClick.AddListener(ExitGame);
        optionsButton.onClick.AddListener(OptionsButton);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    // Loads the options menu
    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");
    }

    // Exit application
    public void ExitGame()
    {
        Debug.Log("Exited");
        Application.Quit();
    }
}
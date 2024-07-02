using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button lvl1;
    public Button lvl2;
    public Button lvl3;
    public Button quitButton;
    public Button optionsButton;

    private void Start()
    {
        lvl1.onClick.AddListener(delegate {LevelSelect(0); });
		lvl2.onClick.AddListener(delegate {LevelSelect(1); });
        lvl3.onClick.AddListener(delegate { LevelSelect(2); });
        quitButton.onClick.AddListener(QuitGame);
        optionsButton.onClick.AddListener(OptionsButton);
    }

    public void LevelSelect(int level)
    {
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("Level01");
                break;
            case 2:
                SceneManager.LoadScene("Level02");
                break;
            case 3:
                SceneManager.LoadScene("Level03");
                break;
        }
    }

    // Loads the options menu
    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");
    }

    // Exit application
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
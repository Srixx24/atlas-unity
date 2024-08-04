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
    private AudioSource audioSource;
    public AudioClip buttonRolloverClip;
    public AudioClip buttonClickClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("MainMenu script requires an AudioSource component on the same GameObject.");
            enabled = false;
            return;
        }

        AddButtonClickListener(Level01, 3);
        AddButtonRolloverListener(Level01);

        AddButtonClickListener(Level02, 4);
        AddButtonRolloverListener(Level02);

        AddButtonClickListener(Level03, 5);
        AddButtonRolloverListener(Level03);

        AddButtonClickListener(exitButton, ExitGame);
        AddButtonRolloverListener(exitButton);

        AddButtonClickListener(optionsButton, OptionsButton);
        AddButtonRolloverListener(optionsButton);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    private void AddButtonClickListener(Button button, int level)
    {
        button.onClick.AddListener(delegate { PlayButtonClickSound(); LoadLevel(level); });
    }

    private void AddButtonClickListener(Button button, System.Action action)
    {
        button.onClick.AddListener(delegate { PlayButtonClickSound(); action(); });
    }

    private void AddButtonRolloverListener(Button button)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { PlayButtonRolloverSound(); });
        trigger.triggers.Add(entry);
    }

    private void PlayButtonRolloverSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(buttonRolloverClip);
        }
        else
        {
            Debug.LogError("AudioSource is null in MainMenu script.");
        }
    }

    private void PlayButtonClickSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
        else
        {
            Debug.LogError("AudioSource is null in MainMenu script.");
        }
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
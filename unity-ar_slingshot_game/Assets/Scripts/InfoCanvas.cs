using UnityEngine;
using UnityEngine.UI;

public class InfoCanvasController : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public Button backButton;

    void Start ()
    {
        backButton.onClick.AddListener(OnBackButton);
    }

    public void OnBackButton()
    {
        // Return to Main Menu
        mainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
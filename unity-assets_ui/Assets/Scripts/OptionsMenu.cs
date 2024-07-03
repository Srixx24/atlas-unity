using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Toggle invertYToggle;
    private CameraController cameraController;
    public int previousSceneIndex;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Load the saved invert Y-axis setting
        invertYToggle.isOn = cameraController.isInverted;
    }

    public void Apply()
    {
        // Save the invert Y-axis setting
        cameraController.isInverted = invertYToggle.isOn;

        // Return to the previous scene
        SceneManager.LoadScene(previousSceneIndex);
    }

    public void Back()
    {
        // Discard any changes and return to the previous scene
        SceneManager.LoadScene(previousSceneIndex);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public GameObject startupCanvas;
    public GameObject gameplayCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject endGameCanvas;
    public SlimeController slimeController;
    public LaunchLogic launchLogic;
    public ScoreKeeper scoreKeeper;


    public void StartupCanvas()
    {
        // Ensure last plane was not saved
        PlaneSelection planeSelection = Object.FindFirstObjectByType<PlaneSelection>();
        if (planeSelection != null)
        {
            planeSelection.ClearPlanes();
        }

        startupCanvas.SetActive(true);
        gameplayCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        endGameCanvas.SetActive(false);

        CleanUpPrefabs();
    }

    public void StartGame()
    {
        startupCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);

        LockInPlane();

        // Start SlimeController
        Object.FindFirstObjectByType<SlimeController>().PopulateTargets();
        if (slimeController != null)
        {
            slimeController.PopulateTargets();
        }

        // Start LaunchLogic
        LaunchLogic launchLogic = Object.FindFirstObjectByType<LaunchLogic>();
        if (launchLogic != null)
        {
            launchLogic.ReadyToLaunch(true); // Allow launching
            launchLogic.Start();
        }
        else
        {
            Debug.LogWarning("!!!!LaunchLogic not set up!!!!");
        }
    }

    public void PlayAgain()
    {
        startupCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        endGameCanvas.SetActive(false);

        ClearPlayField();

        LockInPlane();

        // Start LaunchLogic
        LaunchLogic launchLogic = Object.FindFirstObjectByType<LaunchLogic>();
        if (launchLogic != null)
        {
            launchLogic.ReadyToLaunch(true); // Allow launching
            launchLogic.Start();
        }
    }

    public void PauseGame()
    {
        gameplayCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        gameplayCanvas.SetActive(false);
        endGameCanvas.SetActive(true);
        startupCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);

        launchLogic.ReadyToLaunch(false);

        StartCoroutine(DisplayFinalScore());
    }

    private IEnumerator DisplayFinalScore()
    {
        yield return null; // Wait for last shot
        endGameCanvas.GetComponent<EndGame>().FinalScore();
    }

    public void CleanUpPrefabs()
    {
        // Clear targets and arrow prefabs that could have been
        // instantiate in last game. Will write soon
    }

    public void LockInPlane()
    {
        // Disable further plane detection is disabled
        ARPlaneManager planeManager = Object.FindFirstObjectByType<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.enabled = false;
        }

        // Ensure saved plane is being used
        PlaneSelection planeSelection = Object.FindFirstObjectByType<PlaneSelection>();
        if (planeSelection != null)
        {
            ARPlane selectedPlane = planeSelection.GetSelectedPlane();
            if (selectedPlane != null)
            {
                Debug.Log("Using selected plane: " + selectedPlane.trackableId);
            }
            else
            {
                Debug.LogWarning("No plane selected!");
            }
        }
    }

    public void ClearPlayField()
    {
        Debug.Log("Clearing the play field.");
       
       // Reset arrow count
        launchLogic = Object.FindFirstObjectByType<LaunchLogic>();
        if (launchLogic != null)
        {
            launchLogic.ResetArrowCount();
        }

        // Reset alpha on the arrow UI images
        launchLogic = Object.FindFirstObjectByType<LaunchLogic>();
        if (launchLogic != null)
        {
            launchLogic.ResetReload();
        }

        // Clear the score box
        scoreKeeper = Object.FindFirstObjectByType<ScoreKeeper>();
        if (scoreKeeper != null)
        {
            scoreKeeper.ResetScore();
        }
    }
}

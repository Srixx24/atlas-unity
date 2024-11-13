using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGame : MonoBehaviour
{
    public GameManager gameManager;
    public Button playAgainButton;
    public Button mainMenuButton;
    public ScoreKeeper scoreKeeper;
    public TMP_Text finalScoreText;

    private void Start()
    {
        // Set up button listeners
        if (playAgainButton != null)
        {
            playAgainButton.onClick.RemoveAllListeners();
            playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        FinalScore();
    }

    private void FinalScore()
    {
        if (scoreKeeper != null && finalScoreText != null)
        {
            scoreKeeper.FinalScore(finalScoreText);
        }
    }

    private void OnPlayAgainButtonClicked()
    {
        // Add a UI message that lets the player know the game will start soon
        // with the same plane they selected at the start. Will write soon
        gameManager.PlayAgain();
    }

    private void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
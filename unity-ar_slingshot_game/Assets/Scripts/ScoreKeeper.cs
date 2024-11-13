using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        // Update the score and UI
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            // Updates the displayed score text
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void FinalScore(TMP_Text finalScoreText)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score.ToString();
        }
    }

    public void ResetScore()
    {
        // Reset the score to zero
        score = 0;
        UpdateScoreText();
    }
}
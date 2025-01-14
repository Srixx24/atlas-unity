using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    public int score = 0;
    private int throwsRemaining = 5;
    public GameObject[] pins;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI throwText;

    void Start()
    {
        UpdateScoreText();
        UpdateThrowText();
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (GameObject pin in pins)
        {
            if (other.gameObject == pin)
            {
                Destroy(pin);
                score++;
                UpdateScoreText();
                if (score >= 10)
                    StartCoroutine(ResetCoroutine());
                break;
            }
        }
    }

    public void BallThrown()
    {
        throwsRemaining--;
        UpdateThrowText();
        if (throwsRemaining <= 0)
            StartCoroutine(ResetCoroutine());
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Current Score: " + score;
    }
    
    private void UpdateThrowText()
    {
        throwText.text = "Throws Left: " + throwsRemaining;
    }

    private IEnumerator ResetCoroutine()
    {
        // Flash the score text
        for (int i = 0; i < 5; i++)
        {
            scoreText.color = Color.green;
            yield return new WaitForSeconds(1f);
            scoreText.color = Color.red;
            yield return new WaitForSeconds(1f);
        }

        score = 0;
        throwsRemaining = 5;
        UpdateScoreText();
        UpdateThrowText();

        // Reload the current scene
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
using UnityEngine;

public class TargetHitScore : MonoBehaviour
{
    public ScoreKeeper scoreKeeper;
    private int points;

    public void NotifyHit(string targetType)
    {
        // Determine points based on target type
        switch (targetType)
        {
            case "Pawn":
                points = 10;
                break;
            case "Warrior":
                points = 15;
                break;
            case "King":
                points = 20;
                break;
            default:
                Debug.LogWarning("Unknown target type: " + targetType);
                return;
        }

        // Notify the score keeper
        if (scoreKeeper != null)
        {
            scoreKeeper.AddScore(points);
        }
        else
        {
            Debug.LogError("ScoreKeeper reference is not set in " + gameObject.name);
        }

        Debug.Log($"{targetType} hit! Points: {points}");
    }
}
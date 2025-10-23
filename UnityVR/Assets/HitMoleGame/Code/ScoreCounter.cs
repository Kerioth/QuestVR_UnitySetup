using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public int score;
    [SerializeField]
    public TMP_Text scoreText, bestScoreText;

    public int bestScore;

    public void ResetScore()
    {
        score = 0;
    }
    public void AddScore(int amount = 10)
    {
        score += amount;
    }

    public void SetScoreText()
    {
        GetBestScore(score);

        scoreText.text = score.ToString();
        bestScoreText.text = "Ëó÷øèé: " + bestScore.ToString();
    }

    private void GetBestScore(int playerScore)
    {
        if (PlayerPrefs.HasKey("best"))
        {
            int oldBest = PlayerPrefs.GetInt("best");
            bestScore = playerScore > oldBest ? playerScore : oldBest;
        }
        else {
            bestScore = playerScore;
        }

        PlayerPrefs.SetInt("best", bestScore);
    }


}

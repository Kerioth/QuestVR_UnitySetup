using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public int score;
    [SerializeField]
    public TMP_Text scoreText;

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
        scoreText.text = score.ToString();
    }
}

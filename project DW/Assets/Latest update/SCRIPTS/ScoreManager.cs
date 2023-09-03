using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int killStreak = 0;
    private float killStreakTimer = 0f;
    public float killStreakTimeThreshold = 5f; // Time threshold for a kill streak (in seconds)
    public int killStreakMultiplier = 2; // Multiplier for the score during a kill streak
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI KillStreakText;
    public TMPro.TextMeshProUGUI SpeedMultiplierText;

    public void IncreaseScore()
    {
        // Check if it's a kill streak
        if (killStreak > 0 && killStreakTimer > 0f)
        {
            // Increase the score with the kill streak multiplier and speed multiplier
            int streakScore = (int)(killStreakMultiplier * killStreak * CalculateSpeedMultiplier());
            score += streakScore;
            Debug.Log("Kill streak! Scored " + streakScore + " points.");
        }
        else
        {
            // Increase the score by 1 for a regular kill
            score += 1;
        }

        // Start or continue the kill streak
        StartKillStreak();
        UpdateScoreText();
    }

    public void IncreaseScoreWithHeadshot()
    {
        // Check if it's a kill streak
        if (killStreak > 0 && killStreakTimer > 0f)
        {
            // Increase the score with the kill streak multiplier, headshot multiplier, and speed multiplier
            int streakScore = (int)(killStreakMultiplier * killStreak * 2 * CalculateSpeedMultiplier()); // Multiply by 2 for headshot
            score += streakScore;
            Debug.Log("Kill streak with headshot! Scored " + streakScore + " points.");
        }
        else
        {
            // Increase the score with the headshot multiplier and speed multiplier
            score += (int)(2 * CalculateSpeedMultiplier());
        }

        // Start or continue the kill streak
        StartKillStreak();
        UpdateScoreText();
    }

    private float CalculateSpeedMultiplier()
    {
        float speedMultiplier = 1f;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            AnotherMovement anotherMovement = player.GetComponent<AnotherMovement>();
            if (anotherMovement != null)
            {
                if (anotherMovement.speed > 30f)
                {
                    speedMultiplier = 3f;
                }
                else if (anotherMovement.speed > 20f)
                {
                    speedMultiplier = 2f;
                }
            }
        }
        return speedMultiplier;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void StartKillStreak()
    {
        // Increment the kill streak counter
        killStreak++;

        // Reset the kill streak timer
        killStreakTimer = killStreakTimeThreshold;
    }

    private void Update()
    {
        float speedMultiplier = CalculateSpeedMultiplier();
        SpeedMultiplierText.text = "Speed x: " + CalculateSpeedMultiplier().ToString();
        KillStreakText.text = "Kill Streak: " + killStreak.ToString();
        if (killStreakTimer > 0f)
        {
            // Reduce the kill streak timer
            killStreakTimer -= Time.deltaTime;

            // Check if the kill streak timer has expired
            if (killStreakTimer <= 0f)
            {
                // Reset the kill streak and timer
                killStreak = 0;
                killStreakTimer = 0f;
            }
        }
    }
}
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth = 100f;
    public float currentHealth;

    private ScoreManager scoreManager; // Reference to the ScoreManager script

    void Start()
    {
        currentHealth = startingHealth;
        scoreManager = FindObjectOfType<ScoreManager>(); // Find the ScoreManager script in the scene
    }

    public void TakeDamage(float amount, bool isHeadshot)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die(isHeadshot);
        }
    }

    void Die(bool isHeadshot)
    {
        // TODO: Play death animation and remove enemy from game

        if (scoreManager != null)
        {
            if (isHeadshot)
            {
                scoreManager.IncreaseScoreWithHeadshot(); // Increase score for a headshot kill
            }
            else
            {
                scoreManager.IncreaseScore(); // Increase score for a regular kill
            }
        }
        Destroy(gameObject);
    }
}

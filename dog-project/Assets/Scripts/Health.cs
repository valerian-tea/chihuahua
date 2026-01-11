using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Public function to be called by other scripts to apply damage
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(
            gameObject.name + " took " + damageAmount + " damage. Current Health: " + currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        // Add death effects, animations, or UI updates here
        Destroy(gameObject); // Removes the game object from the scene
    }
}

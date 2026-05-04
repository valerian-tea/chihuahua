using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        // If the Animator might be on a child, you can use:
        // animator = GetComponentInChildren<Animator>();
        // or on a parent:
        // animator = GetComponentInParent<Animator>();
    }

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
        if (animator != null)
        {
            animator.SetTrigger("TakeDamage");
        }

        if (currentHealth <= 0)
        {
            if (animator != null)
            {
                Die();
            }
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator WaitForDeathAnimation()
    {
        // Wait until the animator actually enters the Die state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            yield return null;

        // Wait for the animation to finish
        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        Destroy(gameObject);
    }
}

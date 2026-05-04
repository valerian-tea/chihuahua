using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic attack/hitbox system for both player and enemy attacks.
/// Detects collisions with targets, prevents duplicate hits per attack, and applies damage.
/// </summary>
public class AttackInput : MonoBehaviour
{
    [SerializeField]
    private Collider hitbox;

    [SerializeField]
    private string targetTag = "Enemy"; // Tag of targets this attacker should hit

    [SerializeField]
    private int damage = 20;

    private bool isActive = false;
    private HashSet<Health> targetsInRange = new HashSet<Health>();
    private HashSet<Health> targetsHitThisAttack = new HashSet<Health>();

    /// <summary>
    /// Call this at the start of an attack animation to activate the hitbox.
    /// Damages any targets already in range.
    /// </summary>
    public void EnableHitbox()
    {
        isActive = true;
        targetsHitThisAttack.Clear();

        foreach (var target in targetsInRange)
        {
            Debug.Log($"Dealing damage to {target.gameObject.name} already in hitbox");
            DealDamage(target);
        }
    }

    /// <summary>
    /// Call this at the end of an attack animation to deactivate the hitbox.
    /// </summary>
    public void DisableHitbox()
    {
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} hitbox triggered by {other.gameObject.tag}");
        // Check if the other object has the target tag
        if (!other.CompareTag(targetTag))
            return;

        var target = other.GetComponent<Health>();
        if (!target)
            return;

        targetsInRange.Add(target);

        Debug.Log("isactive: " + isActive + target.gameObject.name);

        // If hitbox is active, damage immediately
        if (isActive)
        {
            DealDamage(target);

            Vector3 spawnPos = transform.position + transform.forward * 1.0f;
            Debug.Log("played impactr");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.GetComponent<Health>();
        if (target)
        {
            targetsInRange.Remove(target);
        }
    }

    private void DealDamage(Health target)
    {
        // Prevent hitting the same target twice in one attack
        if (targetsHitThisAttack.Contains(target))
            return;

        target.TakeDamage(damage);
        targetsHitThisAttack.Add(target);
    }
}

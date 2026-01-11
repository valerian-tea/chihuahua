using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;

    private bool isActive = false;

    private HashSet<Health> enemiesInRange = new HashSet<Health>();
    private HashSet<Health> enemiesHitThisAttack = new HashSet<Health>();

    public void EnableHitbox()
    {
        isActive = true;
        enemiesHitThisAttack.Clear();

        // 🔥 hit enemies already inside
        foreach (var enemy in enemiesInRange)
        {
            DealDamage(enemy);
        }
    }

    public void DisableHitbox()
    {
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        var enemy = other.GetComponent<Health>();
        if (!enemy)
            return;

        enemiesInRange.Add(enemy);

        // Enemy enters during attack
        if (isActive)
        {
            DealDamage(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Health>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private void DealDamage(Health enemy)
    {
        if (enemiesHitThisAttack.Contains(enemy))
            return;

        enemy.TakeDamage(damage);
        enemiesHitThisAttack.Add(enemy);
    }
}

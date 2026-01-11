using System.Collections;
using UnityEngine;

public class BiteInput : MonoBehaviour
{
    public KeyCode biteKey = KeyCode.Space;
    public bool canAttack = true;
    public bool isAttacking { get; private set; }
    public Collider biteHitbox;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
        canAttack = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(biteKey) && canAttack)
        {
            animator.SetTrigger("Bite");
            Debug.Log("Bite triggered");
            isAttacking = true;
            canAttack = false;
            StartCoroutine(ResetCooldown());
        }
    }

    public void BeginAttack()
    {
        biteHitbox.enabled = true;
    }

    public void EndAttack()
    {
        biteHitbox.enabled = false;
    }
}

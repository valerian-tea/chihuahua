using System.Collections;
using UnityEngine;

public class BiteInput : MonoBehaviour
{
    public KeyCode biteKey = KeyCode.Space;
    public bool canAttack = true;
    public bool isAttacking { get; private set; }

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
            isAttacking = true;
            canAttack = false;
            StartCoroutine(ResetCooldown());
        }
    }
}

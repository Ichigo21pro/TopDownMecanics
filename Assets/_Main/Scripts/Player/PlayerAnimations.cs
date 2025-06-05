using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;             // Asigna el Animator en el Inspector
    public PlayerMovement playerMovement; // Asigna el script de movimiento
    public PlayerAttack playerAttack; // Asigna script ataque


    private bool wasAttacking = false;
    void Update()
    {
        // Movimiento
        if (playerMovement != null)
        {
            animator.SetBool("isMoving", playerMovement.isMoving);
        }
        if (playerAttack != null)
        {
            animator.SetBool("tienePistola", playerAttack.pistola);
        }
        // Ataque

        if (playerAttack != null && playerAttack.isAttacking && !wasAttacking)
        {
            animator.SetTrigger("attack");   // Activa la animación de ataque
            wasAttacking = true;
            StartCoroutine(ResetAttackFlag());
        }


    }

    IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.1f); // Espera un poco para permitir la animación
        playerAttack.isAttacking = false;
        wasAttacking = false;
    }
}

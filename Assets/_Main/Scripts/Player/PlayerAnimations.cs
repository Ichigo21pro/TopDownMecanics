using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;             // Asigna el Animator en el Inspector
    public PlayerMovement playerMovement; // Asigna el script de movimiento

    void Update()
    {
        if (playerMovement != null)
        {
            animator.SetBool("isMoving", playerMovement.isMoving);
        }
    }
}

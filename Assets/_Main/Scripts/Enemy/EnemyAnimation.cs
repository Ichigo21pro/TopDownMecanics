using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] public Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Feet Animation (Optional)")]
    public Animator feetAnimator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (feetAnimator == null)
        {
            Transform feet = transform.Find("Feet");
            if (feet != null)
            {
                feetAnimator = feet.GetComponent<Animator>();
            }
        }
    }

    public void SetIsMoving(bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);

        if (feetAnimator != null)
        {
            feetAnimator.SetBool("IsMoving", isMoving);
        }
    }

    public void FlipSprite(float directionX)
    {
        if (directionX != 0)
        {
            spriteRenderer.flipX = directionX < 0;

            if (feetAnimator != null)
            {
                // Si el pie es un sprite que también se debe voltear
                SpriteRenderer feetSprite = feetAnimator.GetComponent<SpriteRenderer>();
                if (feetSprite != null)
                    feetSprite.flipX = directionX < 0;
            }
        }
    }


}

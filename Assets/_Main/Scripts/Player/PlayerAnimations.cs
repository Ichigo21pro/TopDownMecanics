using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Animaciones")]
    public Animator animator;             // Asigna el Animator en el Inspector
    public PlayerMovement playerMovement; // Asigna el script de movimiento
    public PlayerAttack playerAttack; // Asigna script ataque

    private bool wasAttacking = false;
    private bool wasReloading = false;

    [Header("Sonido Caminar")]
    //movimiento
    public AudioClip[] footstepClips; // Tus 10 sonidos de pasos
    private AudioSource audioSource;
    private float stepTimer = 0f;
    public float stepInterval = 0.5f; // Tiempo entre pasos
    public float footstepVolume = 0.8f;
    [Header("Sonido Disparo")]
    //disparo
    public AudioClip disparoClip;
    //Recarga
    public AudioClip regargaClip;

    [Header("Sonido Cuchillo")]
    public AudioClip cuchilloClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        // Movimiento
        if (playerMovement != null)
        {
            animator.SetBool("isMoving", playerMovement.isMoving);
            if (playerMovement.isMoving)
            {
                HandleFootsteps();
            }
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
        if (playerAttack != null && playerAttack.isAttackingPistol && !wasAttacking)
        {
            ReproducirSonidoDisparo();
            animator.SetTrigger("attackPistol");   // Activa la animación de ataque
            wasAttacking = true;
            StartCoroutine(ResetAttackFlag());
        }
        //Recarga
        if (playerAttack != null)
        {
            if (playerAttack.isReloading && !wasReloading)
            {
                ReproducirSonidoRecargar();
                animator.SetTrigger("reload");
                wasReloading = true;
                StartCoroutine(ResetAttackFlag());
            }
            else if (!playerAttack.isReloading && wasReloading)
            {
                wasReloading = false;
            }
        }



    }

    IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.1f); // Espera un poco para permitir la animación
        playerAttack.isAttacking = false;
        playerAttack.isAttackingPistol = false;
        wasAttacking = false;
    }


    // sonido de movimiento al caminar

    void HandleFootsteps()
    {
        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval && footstepClips.Length > 0)
        {
            int index = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[index], footstepVolume);
            stepTimer = 0f;
        }
    }

    // disparo

    void ReproducirSonidoDisparo()
    {
        if (disparoClip != null)
        {
            audioSource.PlayOneShot(disparoClip, 0.3f);
        }
    }

    //recargar

    void ReproducirSonidoRecargar()
    {
        if (disparoClip != null)
        {
            audioSource.PlayOneShot(regargaClip, 0.3f);
        }
    }

    // cuchillo

    void ReproducirSonidoCuchillo()
    {
        if (cuchilloClip != null)
        {
            audioSource.PlayOneShot(cuchilloClip, 0.3f);
        }
    }
}

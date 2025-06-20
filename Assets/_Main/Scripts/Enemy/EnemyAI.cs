using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrulla")]
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public float reachDistance = 0.1f;
    public float waitTime = 0.5f;

    private int currentPointIndex = 0;
    private EnemyAnimation animController;
    private SpriteRenderer spriteRenderer;
    private bool isWaiting = false;
    [Header("Rotación")]
    public float rotationSpeed = 1.5f;
    public float rotationTolerance = 2f; // en grados

    [Header("Detecccion de Player")]
    [SerializeField] private DetectarPlayer detector;


    private void Start()
    {
        animController = GetComponent<EnemyAnimation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        detector = GetComponent<DetectarPlayer>();
    }

    private void Update()
    {
        if (detector != null && detector.jugadorDetectado)
        {
            LookAtPlayer();
            animController?.SetIsMoving(false);
        }
        else if (!isWaiting)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Transform target = patrolPoints[currentPointIndex];
        Vector2 direction = target.position - transform.position;

        // Calcular el ángulo deseado y la rotación objetivo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // Rotar suavemente
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Calcular diferencia angular
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        // Si está alineado (no mover hasta que gire lo suficiente)
        if (angleDifference < rotationTolerance)
        {
            // Mover hacia el punto
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            animController?.SetIsMoving(true);

            // Si llegó al punto
            if (Vector2.Distance(transform.position, target.position) < reachDistance)
            {
                animController?.SetIsMoving(false);
                StartCoroutine(WaitBeforeNextPoint());
            }
        }
        else
        {
            animController?.SetIsMoving(false); // no se mueve mientras gira
        }
    }

    IEnumerator WaitBeforeNextPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        isWaiting = false;
    }


    // ha sido detectado el jugador :

    void LookAtPlayer()
    {
        if (detector.player == null) return;

        Vector2 direction = detector.player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}

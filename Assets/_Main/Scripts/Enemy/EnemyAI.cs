using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public float reachDistance = 0.1f;
    public float waitTime = 1f;
    public float rotationSpeed = 5f;

    private int currentPointIndex = 0;
    private bool isWaiting = false;

    private EnemyAnimation animController;

    private Quaternion initialRotation;
    private Quaternion targetWaitRotation;
    private bool isRotatingDuringWait = false;

    void Start()
    {
        animController = GetComponent<EnemyAnimation>();
    }

    void Update()
    {
        if (!isWaiting)
        {
            Patrol();
        }
        else
        {
            animController?.SetIsMoving(false);
            if (isRotatingDuringWait)
            {
                RotateDuringWait();
            }
        }
    }

    void Patrol()
    {
        Transform target = patrolPoints[currentPointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        // Rotar hacia el punto
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mover hacia el punto
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        animController?.SetIsMoving(true);

        if (Vector2.Distance(transform.position, target.position) < reachDistance)
        {
            animController?.SetIsMoving(false);
            StartCoroutine(WaitBeforeNextPoint());
        }
    }

    IEnumerator WaitBeforeNextPoint()
    {
        isWaiting = true;
        isRotatingDuringWait = true;

        initialRotation = transform.rotation;

        // Giro aleatorio ±90 grados
        float randomSign = Random.value < 0.5f ? 1f : -1f;
        float giroGrados = 90f * randomSign;
        targetWaitRotation = initialRotation * Quaternion.Euler(0, 0, giroGrados);

        float rotationTime = 0.5f; // tiempo para girar suavemente
        float elapsed = 0f;

        while (elapsed < rotationTime)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetWaitRotation, elapsed / rotationTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar rotación final exacta
        transform.rotation = targetWaitRotation;

        isRotatingDuringWait = false;

        // Esperar el resto del tiempo después de girar
        yield return new WaitForSeconds(waitTime - rotationTime);

        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        isWaiting = false;
    }

    void RotateDuringWait()
    {
        // Esta función ya no es necesaria, se hizo dentro del Coroutine
    }
}

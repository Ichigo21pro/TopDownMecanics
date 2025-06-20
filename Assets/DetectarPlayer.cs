using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarPlayer : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleLayers;
    public float visionRange = 8f;
    public float visionAngle = 60f; // en grados

    // varible para parar el movimiento del enemigo
    [HideInInspector] public bool jugadorDetectado = false;
    void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("No se encontró un objeto con el tag 'Player'");
            }
        }
    }
    void Update()
    {
        jugadorDetectado = false; // Reset cada frame

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ¿Está dentro del rango de visión?
        if (distanceToPlayer <= visionRange)
        {
            // Dirección actual del enemigo (por ejemplo, hacia la derecha)
            Vector2 forward = transform.right;

            // Ángulo entre la dirección del enemigo y la dirección hacia el jugador
            float angle = Vector2.Angle(forward, directionToPlayer);

            if (angle < visionAngle / 2f)
            {
                // Verificar que no haya obstáculos entre el enemigo y el jugador
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayers);

                if (hit.collider == null)
                {
                    // Jugador dentro del cono de visión y sin obstáculos

                    jugadorDetectado = true;
                    //MoveTowardsPlayer();
                }
                else
                {
                    
                }

                // Debug visual
                Debug.DrawLine(transform.position, player.position, hit.collider == null ? Color.green : Color.red);
            }
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * 2f);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 forward = transform.right;
        float halfAngle = visionAngle / 2f;

        Quaternion leftRayRotation = Quaternion.Euler(0, 0, -halfAngle);
        Quaternion rightRayRotation = Quaternion.Euler(0, 0, halfAngle);

        Vector3 leftRay = leftRayRotation * forward * visionRange;
        Vector3 rightRay = rightRayRotation * forward * visionRange;

        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}


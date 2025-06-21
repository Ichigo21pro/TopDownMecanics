using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DetectarPlayer : MonoBehaviour
{
    public Transform player;
    public Transform visionOrigin; // <-- Nuevo campo editable para cambiar posición del cono

    public LayerMask obstacleLayers;

    [Range(1f, 20f)]
    public float visionRange = 8f;

    [Range(1f, 360f)]
    public float visionAngle = 60f;

    private bool jugadorDetectado = false;
    public bool JugadorDetectado => jugadorDetectado;

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

        if (visionOrigin == null)
        {
            visionOrigin = this.transform; // Por defecto usa su propia posición
        }
    }

    void Update()
    {
        jugadorDetectado = false;

        Vector2 origin = visionOrigin.position;
        Vector2 directionToPlayer = (player.position - (Vector3)origin).normalized;
        float distanceToPlayer = Vector2.Distance(origin, player.position);

        if (distanceToPlayer <= visionRange)
        {
            Vector2 forward = visionOrigin.right;
            float angle = Vector2.Angle(forward, directionToPlayer);

            if (angle < visionAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, directionToPlayer, distanceToPlayer, obstacleLayers);

                if (hit.collider == null)
                {
                    jugadorDetectado = true;
                }

                Debug.DrawLine(origin, player.position, hit.collider == null ? Color.green : Color.red);
            }
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * 2f);
    }

    void OnDrawGizmosSelected()
    {
        if (visionOrigin == null) return;

        Gizmos.color = Color.yellow;
        Vector3 forward = visionOrigin.right;
        float halfAngle = visionAngle / 2f;

        Quaternion leftRayRotation = Quaternion.Euler(0, 0, -halfAngle);
        Quaternion rightRayRotation = Quaternion.Euler(0, 0, halfAngle);

        Vector3 leftRay = leftRayRotation * forward * visionRange;
        Vector3 rightRay = rightRayRotation * forward * visionRange;

        Gizmos.DrawRay(visionOrigin.position, leftRay);
        Gizmos.DrawRay(visionOrigin.position, rightRay);
        Gizmos.DrawWireSphere(visionOrigin.position, visionRange);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (visionOrigin == null) return;

        Handles.color = new Color(1f, 1f, 0f, 0.2f);
        Vector3 forward = visionOrigin.right;
        Handles.DrawSolidArc(visionOrigin.position, Vector3.forward,
            Quaternion.Euler(0, 0, -visionAngle / 2f) * forward,
            visionAngle, visionRange);
    }
#endif
}
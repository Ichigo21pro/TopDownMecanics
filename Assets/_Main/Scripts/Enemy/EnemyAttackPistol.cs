using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPistol : MonoBehaviour
{
    public DetectarPlayer detector;
    public float rotationSpeed = 1.5f;

    private void Start()
    {
        detector = GetComponent<DetectarPlayer>();
    }

    private void Update()
    {
        if (detector != null && detector.JugadorDetectado)
        {
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        if (detector.player == null) return;

        Vector2 direction = detector.player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // Rotación suave
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

}

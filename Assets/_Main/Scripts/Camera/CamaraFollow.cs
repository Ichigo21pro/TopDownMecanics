using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target;             // El jugador
    public float smoothSpeed = 5f;       // Velocidad de suavizado
    public float mouseOffsetAmount = 2f; // Qué tanto se adelanta hacia el mouse

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (target == null || cam == null) return;

        // Posición base que sigue al jugador
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Obtener la posición del mouse en el mundo
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Dirección del jugador hacia el mouse
        Vector3 directionToMouse = (mouseWorldPos - target.position).normalized;

        // Desplazamiento hacia el mouse
        Vector3 offset = directionToMouse * mouseOffsetAmount;

        // Nueva posición deseada (jugador + desplazamiento hacia mouse)
        Vector3 desiredPosition = targetPosition + new Vector3(offset.x, offset.y, 0);

        // Suavizado
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);

        // Asignar posición final
        transform.position = smoothedPosition;
    }
}

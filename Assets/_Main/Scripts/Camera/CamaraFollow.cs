using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform target;           // El jugador
    public float smoothSpeed = 0.125f; // Suavidad de movimiento
    public Vector3 baseOffset = new Vector3(0, 0, -10f); // Offset básico (usualmente en Z para mantener la cámara)
    public float lookAheadDistance = 2f; // Qué tanto se adelanta la cámara

    void LateUpdate()
    {
        // Obtener dirección del jugador hacia el cursor
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mouseWorldPos - target.position).normalized;

        // Crear un offset dinámico hacia adelante
        Vector3 dynamicOffset = new Vector3(direction.x, direction.y, 0) * lookAheadDistance;

        // Calcular la posición deseada con offset dinámico
        Vector3 desiredPosition = target.position + baseOffset + dynamicOffset;

        // Suavizar el movimiento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Mantener la posición Z de la cámara
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, baseOffset.z);
    }
}

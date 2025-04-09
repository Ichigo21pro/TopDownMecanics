using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform target;           // El jugador
    public float smoothSpeed = 0.125f; // Suavidad de movimiento
    public Vector3 baseOffset = new Vector3(0, 0, -10f); // Offset b�sico (usualmente en Z para mantener la c�mara)
    public float lookAheadDistance = 2f; // Qu� tanto se adelanta la c�mara

    void LateUpdate()
    {
        // Obtener direcci�n del jugador hacia el cursor
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mouseWorldPos - target.position).normalized;

        // Crear un offset din�mico hacia adelante
        Vector3 dynamicOffset = new Vector3(direction.x, direction.y, 0) * lookAheadDistance;

        // Calcular la posici�n deseada con offset din�mico
        Vector3 desiredPosition = target.position + baseOffset + dynamicOffset;

        // Suavizar el movimiento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Mantener la posici�n Z de la c�mara
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, baseOffset.z);
    }
}

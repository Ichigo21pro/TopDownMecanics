using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del personaje
    public Rigidbody2D rb; // Asigna el Rigidbody2D desde el Inspector

    Vector2 movement;

    //
    public bool isMoving;

    void Update()
    {
        ////////////////////////
        // move
        MovimientoPlayer();
        ////////////////////////
        // cursor
        CursorPoint();
    }

    void FixedUpdate()
    {
        // Aplicar el movimiento al Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void CursorPoint()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcular la dirección del mouse al personaje
        Vector2 direction = (mousePos - transform.position).normalized;

        // Calcular el ángulo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplicar la rotación (ajusta si tu sprite apunta hacia la derecha o arriba por defecto)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void MovimientoPlayer()
    {
        // Leer el input del jugador (WASD o flechas)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalizar para que no se mueva más rápido en diagonal
        movement = movement.normalized;
        //////////////////////////////////////////////////////////////
        // Determinar si se está moviendo (simple: si hay input)
        isMoving = movement != Vector2.zero;
    }
}

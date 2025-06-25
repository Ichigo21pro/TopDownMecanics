using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int vidas = 3;

    public void OnHitByBullet()
    {
        vidas--;
        Debug.Log("¡Jugador fue golpeado! Vidas restantes: " + vidas);

        if (vidas <= 0)
        {
            Debug.Log("Jugador ha sido derrotado");
            // Aquí podrías desactivar al jugador, reiniciar escena, mostrar UI, etc.
            // gameObject.SetActive(false); // Ejemplo
        }
    }
}

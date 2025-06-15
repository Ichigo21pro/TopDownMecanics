using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLinterna : MonoBehaviour
{
    [Header("Referencias a las luces de la linterna")]
    public Light2D[] linternas; // Asigna aquí las dos luces Spot

    [Header("Tecla para activar/desactivar")]
    public KeyCode toggleKey = KeyCode.Space;

    [Header("Sistema de batería")]
    public float bateriaMaxima = 100f;
    public float consumoPorSegundo = 5f;
    public float bateriaActual;
    public bool linternaActiva = true;

    [Header("Parpadeo")]
    public float bateriaCritica = 20f;
    public float parpadeoMin = 0.05f;
    public float parpadeoMax = 0.2f;

    private Coroutine parpadeoCoroutine;

    void Start()
    {
        bateriaActual = bateriaMaxima;
        ActualizarEstadoLinternas(linternaActiva);
    }

    void Update()
    {
        // Alternar linterna
        if (Input.GetKeyDown(toggleKey))
        {
            linternaActiva = !linternaActiva;
            ActualizarEstadoLinternas(linternaActiva);

            if (linternaActiva && bateriaActual <= bateriaCritica)
            {
                if (parpadeoCoroutine == null)
                    parpadeoCoroutine = StartCoroutine(Parpadeo());
            }
            else
            {
                DetenerParpadeo();
            }
        }

        // Consumir batería
        if (linternaActiva && bateriaActual > 0)
        {
            bateriaActual -= consumoPorSegundo * Time.deltaTime;

            if (bateriaActual <= 0)
            {
                bateriaActual = 0;
                linternaActiva = false;
                ActualizarEstadoLinternas(false);
                DetenerParpadeo();
            }
            else if (bateriaActual <= bateriaCritica && parpadeoCoroutine == null)
            {
                parpadeoCoroutine = StartCoroutine(Parpadeo());
            }
        }
    }

    void ActualizarEstadoLinternas(bool estado)
    {
        foreach (var luz in linternas)
        {
            if (luz != null)
                luz.enabled = estado;
        }
    }

    void DetenerParpadeo()
    {
        if (parpadeoCoroutine != null)
        {
            StopCoroutine(parpadeoCoroutine);
            parpadeoCoroutine = null;
        }
        // Respeta el estado actual de la linterna
        ActualizarEstadoLinternas(linternaActiva);
    }

    IEnumerator Parpadeo()
    {
        while (bateriaActual <= bateriaCritica && linternaActiva)
        {
            ActualizarEstadoLinternas(false);
            yield return new WaitForSeconds(Random.Range(parpadeoMin, parpadeoMax));
            ActualizarEstadoLinternas(true);
            yield return new WaitForSeconds(Random.Range(parpadeoMin, parpadeoMax));
        }

        ActualizarEstadoLinternas(true);
        parpadeoCoroutine = null;
    }
}

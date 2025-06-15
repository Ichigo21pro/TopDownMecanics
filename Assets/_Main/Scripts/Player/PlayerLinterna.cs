using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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

    [Header("UI")]
    public TextMeshProUGUI textoBateria;

    private Coroutine parpadeoCoroutine;
    [Header("UI de Batería Visual")]
    public Image[] barrasBateria; // 5 imágenes (índice 0 = más llena, índice 4 = más vacía)

    public Color colorVerde = Color.green;
    public Color colorNaranja = new Color(1f, 0.64f, 0f); // naranja
    public Color colorRojo = Color.red;
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

        ActualizarTextoBateria();
        ActualizarBarrasBateria();
    }

    public void ActualizarEstadoLinternas(bool estado)
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

    void ActualizarTextoBateria()
    {
        if (textoBateria != null)
        {
            float porcentaje = (bateriaActual / bateriaMaxima) * 100f;

            // Redondear al múltiplo de 25 más cercano
            int porcentajeRedondeado = Mathf.RoundToInt(porcentaje / 25f) * 25;
            porcentajeRedondeado = Mathf.Clamp(porcentajeRedondeado, 0, 100);

            textoBateria.text = porcentajeRedondeado.ToString() + "%";
        }
    }

    void ActualizarBarrasBateria()
    {
        if (barrasBateria == null || barrasBateria.Length == 0) return;

        float porcentaje = bateriaActual / bateriaMaxima;
        int barrasActivas = Mathf.CeilToInt(porcentaje * barrasBateria.Length);

        for (int i = 0; i < barrasBateria.Length; i++)
        {
            bool activa = i < barrasActivas;
            barrasBateria[i].enabled = activa;

            // Cambiar color según cuántas quedan activas
            if (activa)
            {
                if (barrasActivas <= 1)
                    barrasBateria[i].color = colorRojo;
                else if (barrasActivas <= 3)
                    barrasBateria[i].color = colorNaranja;
                else
                    barrasBateria[i].color = colorVerde;
            }
        }
    }
}

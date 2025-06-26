using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAttackPistol : MonoBehaviour
{
    [Header("Script Deteccion")]
    [SerializeField] private DetectarPlayer detector; // Script externo que ya detecta al jugador
    private bool yaDetectado = false;
    [Header("Persecución")]
    [SerializeField] private Transform objetivo;
    private NavMeshAgent agente;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) objetivo = player.transform;

        agente = GetComponent<NavMeshAgent>();
        if (agente != null)
        {
            agente.updateRotation = false;
            agente.updateUpAxis = false;
        }
    }
    void Update()
    {
        if (!yaDetectado && detector != null && detector.JugadorDetectado)
        {
            yaDetectado = true;
        }

        if (yaDetectado && objetivo != null && agente != null)
        {
            agente.SetDestination(objetivo.position);
        }
    }
}
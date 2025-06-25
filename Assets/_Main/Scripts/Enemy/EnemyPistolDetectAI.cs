using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAttackPistol : MonoBehaviour
{
    public DetectarPlayer detector;
    private Transform target;
    private NavMeshAgent agent;

    public float rotationSpeed = 10f;
    public bool haDetectadoAlJugador = false;

    public float distanciaMinima = 8f; // Distancia mínima que debe mantener con el jugador

    private bool estabaMoviendose = false; // Almacena el último estado
    private EnemyAnimation animController;
    public float velocidadPersecucion = 4.0f; // ajusta según lo que necesites

    void Start()
    {
        detector = GetComponent<DetectarPlayer>();
        agent = GetComponent<NavMeshAgent>();
        animController = GetComponent<EnemyAnimation>();
        agent.speed = velocidadPersecucion;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {

        if (target == null) return;

        if ((detector != null && detector.JugadorDetectado) || haDetectadoAlJugador)
        {
            haDetectadoAlJugador = true;
            

            float distancia = Vector3.Distance(transform.position, target.position);

            if (distancia > distanciaMinima + 0.5f)
            {
                agent.SetDestination(target.position);
            }
            else if (distancia < distanciaMinima - 0.5f)
            {
                Vector3 direccionAlejarse = transform.position - target.position;
                Vector3 destinoAlejarse = transform.position + direccionAlejarse.normalized * 2f;
                agent.SetDestination(destinoAlejarse);
            }
            else
            {
                agent.ResetPath();
            }

            

            // Rotación hacia el jugador
            Vector3 direction = target.position - transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
            }


        }
       

        bool estaMoviendoseAhora = agent.velocity.magnitude > 0.1f;
        if (animController != null && estabaMoviendose != estaMoviendoseAhora)
        {
            animController.SetIsMoving(estaMoviendoseAhora);
            estabaMoviendose = estaMoviendoseAhora;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackPistol : MonoBehaviour
{
    public DetectarPlayer detector;
    [SerializeField] Transform target;
    NavMeshAgent agent;
    [Header("Como gira para mirarte")]
    public float rotationSpeed = 3f;
    [Header("Tiempo de escape")]
    public float tiempoDeEscape = 5f;
    private bool aSidoDetectado = false;
    Coroutine resetCoroutine = null;
    [SerializeField]  private EnemyAnimation animController;

    private void Start()
    {
        animController = GetComponent<EnemyAnimation>();
        detector = GetComponent<DetectarPlayer>();
        // Buscar al jugador automáticamente por tag
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("No se encontró un objeto con el tag 'Player'");
            }
        }
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        // Si el jugador es detectado, marcamos como detectado y cancelamos cualquier temporizador
        if (detector != null && detector.JugadorDetectado)
        {
            if (!aSidoDetectado)
            {
                aSidoDetectado = true;
            }

        }

        if (aSidoDetectado && target != null)
        {
            animController?.SetIsMoving(true);
            agent.SetDestination(target.position);
            LookAtPlayer();
            
        }

    }


    ////
    void LookAtPlayer()
    {
        if (target == null) return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

}

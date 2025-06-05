using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuertaInteract : MonoBehaviour
{
    public float interactRadius = 1.5f;
    public LayerMask doorLayer;

    // Para guardar las puertas que estaban activas anteriormente
    private HashSet<Transform> previousDoors = new HashSet<Transform>();
    // Estado de la puerta
    private Dictionary<Transform, bool> doorStates = new Dictionary<Transform, bool>();


    void Update()
    {
        // Buscar puertas cercanas
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, interactRadius, doorLayer);
        HashSet<Transform> currentDoors = new HashSet<Transform>();

        foreach (Collider2D col in nearby)
        {
            Transform door = col.transform;
            currentDoors.Add(door);

            // Activar outlines
            SetOutlineActive(door, true);
        }

        // Desactivar outlines de puertas que ya no están cerca
        foreach (Transform prevDoor in previousDoors)
        {
            if (!currentDoors.Contains(prevDoor))
            {
                SetOutlineActive(prevDoor, false);
            }
        }

        // Guardar las puertas actuales para el próximo frame
        previousDoors = currentDoors;

        // Acciones al presionar E (por ejemplo, aplicar fuerza)
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Collider2D col in nearby)
            {
                HingeJoint2D hinge = col.GetComponent<HingeJoint2D>();
                if (hinge != null && hinge.useLimits)
                {
                    bool isOpen = false;
                    if (!doorStates.TryGetValue(col.transform, out isOpen))
                    {

                        doorStates[col.transform] = false; // Inicial cerrado
                    }

                    isOpen = !isOpen;
                    doorStates[col.transform] = isOpen;

                    JointAngleLimits2D limits = hinge.limits;
                    JointMotor2D motor = hinge.motor;

                    // Dirección desde la puerta hacia el jugador
                    Vector2 toPlayer = (transform.position - col.transform.position).normalized;

                    // Vector de apertura "frontal" de la puerta (puede ser right, up, etc. según el diseño)
                    Vector2 doorRight = col.transform.right;

                    // Ángulo entre la dirección de la puerta y la posición del jugador
                    float angleToPlayer = Vector2.SignedAngle(doorRight, toPlayer);

                    // Decidir el sentido de apertura
                    bool openClockwise = angleToPlayer < 0;

                    if (!isOpen)
                    {
                        if (openClockwise)
                        {
                            limits.min = -90;
                            limits.max = 0;
                            motor.motorSpeed = 150; // sentido horario
                            
                        }
                        else
                        {
                            limits.min = 0;
                            limits.max = 90;
                            motor.motorSpeed = -150; // sentido antihorario
                        }
                        hinge.limits = limits;
                        motor.maxMotorTorque = 1000;
                        hinge.motor = motor;
                        hinge.useMotor = true;
                    }
                    else
                    {
                        // Siempre volver a ángulo 0 al cerrar
                        limits.min = -90;
                        limits.max = 90;
                        hinge.limits = limits;

                        motor.motorSpeed = openClockwise ? -100 : 100;
                        motor.maxMotorTorque = 1000;
                        hinge.motor = motor;
                        hinge.useMotor = true;
                    }
                }
            }
        }


    }

    // Activar o desactivar los outlines de una puerta
    void SetOutlineActive(Transform doorTransform, bool active)
    {
        DoorOutlineController controller = doorTransform.GetComponent<DoorOutlineController>();
        if (controller != null)
        {
            controller.SetOutlinesActive(active);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}


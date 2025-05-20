using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuertaInteract : MonoBehaviour
{
    public float interactRadius = 1.5f;
    public LayerMask doorLayer;
    public float openForce = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Busca puertas cercanas
            Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, interactRadius, doorLayer);

            foreach (Collider2D col in nearby)
            {
                Rigidbody2D rb = col.attachedRigidbody;

                if (rb != null && col.GetComponent<HingeJoint2D>())
                {
                    Vector2 direction = (transform.position - col.transform.position).normalized; 
                    rb.AddForce(direction * openForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    // Dibuja el radio de interacción en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}

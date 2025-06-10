using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDamage : MonoBehaviour
{
    public int damage = 50;
    public string enemyTag = "Enemy";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            EnemyHealth health = collision.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject); // Destruye la bala al impactar
        }
        if (collision.CompareTag("Wall") || collision.CompareTag("Interact")  || collision.gameObject.layer == LayerMask.NameToLayer("BlockBullet"))
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 3f); // Destruir la bala si no impacta después de cierto tiempo
    }
}

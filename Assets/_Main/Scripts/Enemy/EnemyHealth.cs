using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;      // Vida máxima
    [SerializeField] private int currentHealth;
    [Header("Drop")]
    [SerializeField] private GameObject pistolaPrefab;  // Asigna el prefab en el Inspector
    [Range(0f, 1f)]
    public float dropProbability = 0.2f; // 20% de probabilidad


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} recibió {damage} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Intentar dropear pistola con 20% de probabilidad
        if (pistolaPrefab != null && Random.value <= dropProbability)
        {
            Instantiate(pistolaPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

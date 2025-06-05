using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public int damage = 25;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    public Collider2D attackRangeCollider;

    public bool isAttacking; // <- Este flag será leído por PlayerAnimations

    [SerializeField] private Chest InteractObjectScript;
    public bool pistola;


    void Start()
    {
        

        if (attackRangeCollider == null)
        {
            attackRangeCollider = GetComponentInChildren<Collider2D>();
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (pistola)
            {
                AttackPistola();
            }
            else
            {
                Attack();
            }
        }
    }

    // ataque de cuchillo
    void Attack()
    {
        isAttacking = true; // <- Activa el flag una vez

        // Creamos una copia para evitar modificar la lista mientras la recorremos
        foreach (GameObject enemy in new List<GameObject>(enemiesInRange))
        {
            if (enemy != null)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);

                }
            }
            else
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    void AttackPistola()
    { 
    
    }

        // Collider si el tag es enemigo
        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            if (!enemiesInRange.Contains(other.gameObject))
                enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            if (enemiesInRange.Contains(other.gameObject))
                enemiesInRange.Remove(other.gameObject);
        }
    }
}

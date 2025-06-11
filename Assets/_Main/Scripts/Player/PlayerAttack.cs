using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public int damageKnife = 25;


    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint;     // Punto desde donde dispara
    public float bulletSpeed = 10f; // Velocidad de la bala

    private List<GameObject> enemiesInRange = new List<GameObject>();
    public Collider2D attackRangeCollider;

    public bool isAttacking; // <- Este flag será leído por PlayerAnimations
    public bool isAttackingPistol; // <- Este flag será leído por PlayerAnimations

    [SerializeField] private Chest InteractObjectScript;
    public bool pistola;
    public bool yaTienePistola;

    public int balas = 25;
    public int cantidadCargador = 25;
    public int cargadores = 3;
    public bool isReloading = false;
    public float reloadTime = 0.35f; // Tiempo de recarga en segundos

    public TMP_Text TextoTMPBalas;
    public TMP_Text TextoTMPCargadores;
    private bool MostrarBalas=true;


    void Start()
    {

        if (attackRangeCollider == null)
        {
            attackRangeCollider = GetComponentInChildren<Collider2D>();
        }
    }

    void Update()
    {
        if(MostrarBalas && pistola)
        {
            TextoTMPBalas.text = "" + balas;
            TextoTMPCargadores.text = "" + cargadores;
            MostrarBalas = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (pistola)
            {
                if (!isReloading)
                {
                    if (balas > 0)
                    {
                        AttackPistola();
                        balas--;
                        TextoTMPBalas.text = "" + balas;
                        TextoTMPCargadores.text = "" + cargadores;
                    }
                    else if (cargadores > 0)
                    {
                        StartCoroutine(Reload());
                    }
                    else
                    {
                        Debug.Log("Sin balas y sin cargadores");
                    }
                }
            }
            else
            {
                Attack();
            }
        }
        if (yaTienePistola && Input.GetKeyDown(KeyCode.Alpha1))
        {
            pistola = false; 
        }
        if (yaTienePistola && Input.GetKeyDown(KeyCode.Alpha2))
        {
            pistola = true;
        }
        if (pistola && Input.GetKeyDown(KeyCode.R) && cargadores > 0 && balas < cantidadCargador)
        {
            StartCoroutine(Reload());
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
                    enemyHealth.TakeDamage(damageKnife);

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
        isAttackingPistol = true;

        // Obtener dirección hacia el mouse (igual que en CursorPoint)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = (mousePos - firePoint.position).normalized;

        // Instanciar la bala con la rotación hacia el mouse
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

        // Aplicar la velocidad
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed;
        }
    }

    // Recargar

    private IEnumerator Reload()
    {
        if (isReloading) yield break; // <- Previene múltiples llamadas simultáneas

        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        if (cargadores > 0)
        {
            cargadores--;
            balas = cantidadCargador;
            TextoTMPCargadores.text = "" + cargadores;
            TextoTMPBalas.text = "" + balas;
        }

        isReloading = false;

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
